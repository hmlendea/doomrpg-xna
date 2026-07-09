using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NuciXNA.Primitives;

using DoomRPG.DataAccess.Repositories;
using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.GameLogic.Mapping;
using DoomRPG.Models;
using DoomRPG.Models.Enumerations;
using DoomRPG.Settings;

namespace DoomRPG.GameLogic.GameManagers
{
    public sealed class GameManager : IGameManager
    {
        IEnumerable<Ammunition> ammunitions;
        IEnumerable<Wall> wallDefinitions;
        IEnumerable<Weapon> weaponDefinitions;

        readonly ILevelManager levelManager;
        readonly IMobManager mobManager;
        readonly IPlayerManager playerManager;
        readonly Random rng = new();

        public GameManager()
        {
            levelManager = new LevelManager();
            mobManager = new MobManager(levelManager);
            playerManager = new PlayerManager(levelManager);
        }

        public void LoadContent()
        {
            levelManager.LoadContent("test"); // TODO: Remove hardcoding.
            mobManager.LoadContent();
            playerManager.LoadContent();

            string ammoPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "ammo.xml");
            string wallPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "walls.xml");

            string weaponPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "weapons.xml");

            AmmunitionRepository ammoRepository = new(ammoPath);
            WallRepository wallRepository = new(wallPath);
            WeaponRepository weaponRepository = new(weaponPath);

            ammunitions = ammoRepository.GetAll().ToDomainModels();
            wallDefinitions = wallRepository.GetAll().ToDomainModels();
            weaponDefinitions = weaponRepository.GetAll().ToDomainModels();
        }

        public void UnloadContent()
        {
            levelManager.UnloadContent();
            mobManager.UnloadContent();
            playerManager.UnloadContent();

            ammunitions = [];
            wallDefinitions = [];
            weaponDefinitions = [];
        }

        public void Update(float elapsedSeconds)
        {
            levelManager.Update(elapsedSeconds);
            mobManager.Update(elapsedSeconds);
            playerManager.Update(elapsedSeconds);
        }

        public void MovePlayer(MovementDirection direction)
        {
            bool moved = playerManager.MovePlayer(direction);

            if (moved)
            {
                levelManager.AdvanceTurn();
            }
        }

        public AttackResult Attack()
        {
            Weapon weapon = GetEquippedWeapon();

            if (weapon is null)
            {
                return new AttackResult { Outcome = AttackOutcome.NoAmmo };
            }

            if (!string.IsNullOrEmpty(weapon.AmmunitionId) && weapon.AmmoPerShot > 0)
            {
                bool ammoSpent = playerManager.SpendAmmo(weapon.AmmunitionId, weapon.AmmoPerShot);

                if (!ammoSpent)
                {
                    return new AttackResult { Outcome = AttackOutcome.NoAmmo };
                }
            }

            levelManager.AdvanceTurn();

            MobInstance target = FindTargetInView();

            if (target is null)
            {
                return new AttackResult { Outcome = AttackOutcome.NoTarget };
            }

            Mob mobDef = mobManager.GetMobDefinition(target.MobId);
            Player player = playerManager.GetPlayer();

            AttackOutcome outcome = ResolveHit(player, weapon, mobDef);

            if (outcome == AttackOutcome.Missed)
            {
                return new AttackResult { Outcome = AttackOutcome.Missed, MobName = mobDef.Name };
            }

            int damage = CalculateDamage(player, weapon, mobDef, outcome == AttackOutcome.Crit || outcome == AttackOutcome.CritKill);
            mobManager.ApplyDamageToMob(target, damage);

            int ammoRemaining = 0;
            if (!string.IsNullOrEmpty(weapon.AmmunitionId))
            {
                player.AmmoCounts.TryGetValue(weapon.AmmunitionId, out ammoRemaining);
            }

            if (target.CurrentHealth <= 0)
            {
                levelManager.RemoveMob(target.Id);
                mobManager.RemoveMob(target.Id);
                playerManager.AddExperience(mobDef.Health);

                return new AttackResult
                {
                    Outcome = outcome == AttackOutcome.Crit ? AttackOutcome.CritKill : AttackOutcome.Kill,
                    Damage = damage,
                    MobName = mobDef.Name,
                    AmmoRemaining = ammoRemaining
                };
            }

            return new AttackResult
            {
                Outcome = outcome,
                Damage = damage,
                MobName = mobDef.Name,
                AmmoRemaining = ammoRemaining
            };
        }

        /// <summary>
        /// Finds the nearest mob directly in the player's view using DDA ray marching.
        /// </summary>
        MobInstance FindTargetInView()
        {
            Player player = playerManager.GetPlayer();

            float rayX = player.Position.X;
            float rayY = player.Position.Y;
            float dirX = player.Direction.X;
            float dirY = player.Direction.Y;

            // Normalise direction
            float len = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            if (len < 0.0001f) return null;
            dirX /= len;
            dirY /= len;

            const float StepSize = 0.5f;
            const int MaxSteps = 20;

            for (int step = 1; step <= MaxSteps; step++)
            {
                float checkX = rayX + dirX * step * StepSize;
                float checkY = rayY + dirY * step * StepSize;

                int tileX = (int)Math.Floor(checkX);
                int tileY = (int)Math.Floor(checkY);

                // Stop at walls
                if (levelManager.GetWall(tileX, tileY) is not null)
                {
                    break;
                }

                MobInstance mob = levelManager
                    .GetMobs()
                    .FirstOrDefault(m => m.Position.X == tileX && m.Position.Y == tileY && !m.IsFriendly);

                if (mob is not null)
                {
                    return mob;
                }
            }

            return null;
        }

        /// <summary>
        /// Hit check formula derived from original game's u.a(attacker, weapon, defender, distance).
        /// Stats scaled to 0-255 fixed-point range where 128 = balanced (attacker == defender).
        /// </summary>
        AttackOutcome ResolveHit(Player player, Weapon weapon, Mob mob)
        {
            // Scale player accuracy to original 8-bit range (Accuracy 1 → 20, grows with upgrades)
            int attackerAcc = Math.Clamp(player.Accuracy * 20, 1, 255);

            // Derive mob evasion from its HP: squishier mobs are harder to track
            int defenderEvasion = Math.Clamp(200 - mob.Health / 2, 10, 200);

            // Original formula: hitChance = (attacker.acc * 128 / defender.evasion) + (weapon.d * 65536 / 51200)
            int hitChance = attackerAcc * 128 / defenderEvasion
                          + weapon.AccuracyBonus * 65536 / 51200;

            // Roll 0-255
            int roll = rng.Next(256);

            if (roll >= hitChance)
            {
                return AttackOutcome.Missed;
            }

            // Crit threshold: hitChance * 8 / 5120 (rare at low levels, scales with accuracy investment)
            int critThreshold = hitChance * 8 / 5120;

            return roll < critThreshold ? AttackOutcome.Crit : AttackOutcome.Hit;
        }

        /// <summary>
        /// Damage formula derived from original game's u.a(attacker, weapon, defender, param, distance).
        /// </summary>
        int CalculateDamage(Player player, Weapon weapon, Mob mob, bool isCrit)
        {
            // Weapon damage range ±15% (original had explicit min/max per weapon)
            int minDamage = weapon.Damage * 85 / 100;
            int maxDamage = weapon.Damage * 115 / 100;

            // Random roll 0-255 to pick damage in range (original formula)
            int dmgRoll = rng.Next(256);
            int baseDamage = minDamage + dmgRoll * (maxDamage - minDamage) / 256;

            // Scale by attacker strength vs mob defence
            // Original: damage = base * (attacker.f / defender.e), both 0-255
            int attackerStr = Math.Clamp(player.Strength * 20, 1, 255);
            int mobDefense  = Math.Clamp(mob.Health / 5, 5, 200);

            int damage = baseDamage * attackerStr / mobDefense;

            // Clamp to [1, 999] matching original game
            damage = Math.Clamp(damage, 1, 999);

            if (isCrit)
            {
                damage *= 2;
                damage = Math.Clamp(damage, 1, 999);
            }

            return damage;
        }

        public void RotatePlayer(float angle)
            => playerManager.RotatePlayer(angle);

        public Size2D GetLevelSize()
            => levelManager.GetSize();

        public int GetTurnNumber()
            => levelManager.GetTurnNumber();

        public Colour GetLevelCeilingColour()
            => levelManager.GetCeilingColour();

        public Colour GetLevelFloorColour()
            => levelManager.GetFloorColour();

        public IEnumerable<Wall> GetLevelWallDefinitions()
        {
            IEnumerable<WallInstance> walls = levelManager.GetWalls();

            return wallDefinitions.Where(wallDefinition => walls.Any(wall => wall.WallId.Equals(wallDefinition.Id)));
        }

        public IEnumerable<WallInstance> GetWalls()
            => levelManager.GetWalls();

        public Wall GetWallDefinition(string id)
            => wallDefinitions.FirstOrDefault(wall => wall.Id.Equals(id));

        public WallInstance GetWall(int x, int y)
            => levelManager.GetWall(x, y);

        public IEnumerable<TerminalInstance> GetTerminalInstances()
            => levelManager.GetTerminals();

        public TerminalInstance GetTerminalAtPosition(int x, int y)
            => levelManager.GetTerminalAtPosition(x, y);

        public string InteractWithTerminal()
        {
            Player player = playerManager.GetPlayer();

            float dirX = player.Direction.X;
            float dirY = player.Direction.Y;
            float len = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            if (len < 0.0001f)
            {
                return null;
            }

            dirX /= len;
            dirY /= len;

            int tileX = (int)Math.Floor(player.Position.X + dirX);
            int tileY = (int)Math.Floor(player.Position.Y + dirY);

            TerminalInstance terminal = levelManager.GetTerminalAtPosition(tileX, tileY);
            return terminal?.Text;
        }

        public IEnumerable<Weapon> GetWeaponDefinitions()
            => weaponDefinitions;

        public Weapon GetWeaponDefinition(string id)
            => weaponDefinitions.FirstOrDefault(weapon => weapon.Id.Equals(id));

        public IEnumerable<Ammunition> GetAmmunitionDefinitions()
            => ammunitions;

        public void AddAmmo(string ammoId, int amount)
            => playerManager.AddAmmo(ammoId, amount);

        public bool SpendAmmo(string ammoId, int amount)
            => playerManager.SpendAmmo(ammoId, amount);

        public void GiveWeapon(string weaponId)
            => playerManager.GiveWeapon(weaponId);

        public bool SelectWeapon(string weaponId)
            => playerManager.SelectWeapon(weaponId);

        public bool SelectWeaponBySlot(int slot)
            => playerManager.SelectWeaponBySlot(slot);

        public void CycleWeaponNext()
            => playerManager.CycleWeaponNext();

        public void CycleWeaponPrevious()
            => playerManager.CycleWeaponPrevious();

        public Weapon GetEquippedWeapon()
        {
            string equippedId = playerManager.GetEquippedWeaponId();

            if (equippedId is null)
            {
                return null;
            }

            return weaponDefinitions.FirstOrDefault(weapon => weapon.Id.Equals(equippedId));
        }

        public void AddExperience(int amount)
            => playerManager.AddExperience(amount);

        public bool AllocateStat(StatType stat)
            => playerManager.AllocateStat(stat);

        public Player GetPlayer()
            => playerManager.GetPlayer();

        public IEnumerable<MobInstance> GetMobInstances()
            => mobManager.GetMobInstances();

        public Mob GetMobDefinition(string id)
            => mobManager.GetMobDefinition(id);
    }
}
