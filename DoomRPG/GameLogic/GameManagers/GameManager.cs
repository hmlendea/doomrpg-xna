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
        IEnumerable<WorldObject> worldObjectDefinitions;

        readonly ILevelManager levelManager;
        readonly IMobManager mobManager;
        readonly IPlayerManager playerManager;
        readonly Random randomNumberGenerator = new();

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

            string worldObjectPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "objects.xml");

            AmmunitionRepository ammoRepository = new(ammoPath);
            WallRepository wallRepository = new(wallPath);
            WeaponRepository weaponRepository = new(weaponPath);
            WorldObjectRepository worldObjectRepository = new(worldObjectPath);

            ammunitions = ammoRepository.GetAll().ToDomainModels();
            wallDefinitions = wallRepository.GetAll().ToDomainModels();
            weaponDefinitions = weaponRepository.GetAll().ToDomainModels();
            worldObjectDefinitions = worldObjectRepository.GetAll().ToDomainModels();

            foreach (WorldObjectInstance worldObjectInstance in levelManager.GetWorldObjects())
            {
                WorldObject worldObjectDefinition = worldObjectDefinitions
                    .FirstOrDefault(worldObject => worldObject.Id.Equals(worldObjectInstance.WorldObjectId));

                if (worldObjectDefinition is not null)
                {
                    worldObjectInstance.CurrentHealth = worldObjectDefinition.Health;
                }
            }

            foreach (WallInstance wallInstance in levelManager.GetWalls())
            {
                Wall wallDefinition = wallDefinitions.FirstOrDefault(wall => wall.Id.Equals(wallInstance.WallId));

                if (wallDefinition is not null && wallDefinition.IsDoor)
                {
                    wallInstance.IsDoor = true;
                }
            }
        }

        public void UnloadContent()
        {
            levelManager.UnloadContent();
            mobManager.UnloadContent();
            playerManager.UnloadContent();

            ammunitions = [];
            wallDefinitions = [];
            weaponDefinitions = [];
            worldObjectDefinitions = [];
        }

        public void Update(float elapsedSeconds)
        {
            levelManager.Update(elapsedSeconds);
            mobManager.Update(elapsedSeconds);
            playerManager.Update(elapsedSeconds);
        }

        public MoveResult MovePlayer(MovementDirection direction)
        {
            Player playerForCheck = playerManager.GetPlayer();
            int dirX = (int)Math.Round(playerForCheck.Direction.X);
            int dirY = (int)Math.Round(playerForCheck.Direction.Y);

            (int dx, int dy) = direction switch
            {
                MovementDirection.North => (dirX, dirY),
                MovementDirection.South => (-dirX, -dirY),
                MovementDirection.West => (dirY, -dirX),
                MovementDirection.East => (-dirY, dirX),
                _ => (0, 0)
            };

            int targetX = (int)(playerForCheck.Position.X + dx);
            int targetY = (int)(playerForCheck.Position.Y + dy);

            WorldObjectInstance blockingWorldObject = levelManager.GetWorldObjectAtPosition(targetX, targetY);

            if (blockingWorldObject is not null)
            {
                WorldObject blockingDefinition = worldObjectDefinitions
                    .FirstOrDefault(wo => wo.Id.Equals(blockingWorldObject.WorldObjectId));

                if (blockingDefinition is not null && blockingDefinition.BlocksMovement)
                {
                    return new MoveResult();
                }
            }

            bool moved = playerManager.MovePlayer(direction);

            if (!moved)
            {
                return new MoveResult();
            }

            levelManager.AdvanceTurn();

            Player player = playerManager.GetPlayer();

            int playerTileX = (int)Math.Floor(player.Position.X);
            int playerTileY = (int)Math.Floor(player.Position.Y);

            WorldObjectInstance worldObjectAtTile = levelManager.GetWorldObjectAtPosition(playerTileX, playerTileY);

            if (worldObjectAtTile is null)
            {
                return new MoveResult();
            }

            WorldObject worldObjectDefinition = worldObjectDefinitions
                .FirstOrDefault(worldObject => worldObject.Id.Equals(worldObjectAtTile.WorldObjectId));

            if (worldObjectDefinition is null || (worldObjectDefinition.HealAmount <= 0 && worldObjectDefinition.ArmourAmount <= 0 && string.IsNullOrEmpty(worldObjectDefinition.WeaponId) && string.IsNullOrEmpty(worldObjectDefinition.KeyId) && string.IsNullOrEmpty(worldObjectDefinition.AmmoId)))
            {
                return new MoveResult();
            }

            if (!string.IsNullOrEmpty(worldObjectDefinition.WeaponId))
            {
                playerManager.GiveWeapon(worldObjectDefinition.WeaponId);
                levelManager.RemoveWorldObject(worldObjectAtTile.Id);

                return new MoveResult
                {
                    PickedUpObjectName = worldObjectDefinition.Name,
                    PickedUpWeaponId = worldObjectDefinition.WeaponId
                };
            }

            if (!string.IsNullOrEmpty(worldObjectDefinition.KeyId))
            {
                playerManager.GiveKey(worldObjectDefinition.KeyId);
                levelManager.RemoveWorldObject(worldObjectAtTile.Id);

                return new MoveResult
                {
                    PickedUpObjectName = worldObjectDefinition.Name,
                    PickedUpKeyId = worldObjectDefinition.KeyId
                };
            }

            if (!string.IsNullOrEmpty(worldObjectDefinition.AmmoId))
            {
                playerManager.AddAmmo(worldObjectDefinition.AmmoId, worldObjectDefinition.AmmoAmount);
                levelManager.RemoveWorldObject(worldObjectAtTile.Id);

                return new MoveResult
                {
                    PickedUpObjectName = worldObjectDefinition.Name,
                    PickedUpAmmoId = worldObjectDefinition.AmmoId,
                    PickedUpAmmoAmount = worldObjectDefinition.AmmoAmount
                };
            }

            int actualHeal = Math.Min(worldObjectDefinition.HealAmount, player.MaxHealth - player.Health);

            if (worldObjectDefinition.ArmourAmount > 0)
            {
                int actualArmour = Math.Min(worldObjectDefinition.ArmourAmount, player.MaxArmour - player.Armour);
                playerManager.RestoreArmour(worldObjectDefinition.ArmourAmount);
                levelManager.RemoveWorldObject(worldObjectAtTile.Id);

                return new MoveResult
                {
                    PickedUpObjectName = worldObjectDefinition.Name,
                    ArmourAmountReceived = actualArmour
                };
            }

            playerManager.Heal(worldObjectDefinition.HealAmount);
            levelManager.RemoveWorldObject(worldObjectAtTile.Id);

            return new MoveResult
            {
                PickedUpObjectName = worldObjectDefinition.Name,
                HealAmountReceived = actualHeal
            };
        }

        public AttackResult Attack()
        {
            Weapon weapon = GetEquippedWeapon();

            if (weapon is null)
            {
                return new AttackResult { Outcome = AttackOutcome.NoAmmo };
            }

            MobInstance firstMobInView = FindFirstMobInView();

            if (firstMobInView is not null && firstMobInView.IsFriendly)
            {
                return new AttackResult { Outcome = AttackOutcome.NoTarget };
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

            if (weapon.Id.Equals("axe"))
            {
                Player playerForAxe = playerManager.GetPlayer();
                float axeDirX = playerForAxe.Direction.X;
                float axeDirY = playerForAxe.Direction.Y;
                float axeMag = (float)Math.Sqrt(axeDirX * axeDirX + axeDirY * axeDirY);

                if (axeMag > 0.0001f)
                {
                    axeDirX /= axeMag;
                    axeDirY /= axeMag;

                    int facingTileX = (int)Math.Floor(playerForAxe.Position.X + axeDirX);
                    int facingTileY = (int)Math.Floor(playerForAxe.Position.Y + axeDirY);
                    WallInstance jammedWall = levelManager.GetWall(facingTileX, facingTileY);

                    if (jammedWall is not null && jammedWall.WallId.Equals("jammed_door"))
                    {
                        levelManager.RemoveWallAtPosition(facingTileX, facingTileY);
                        return new AttackResult { Outcome = AttackOutcome.JammedDoorDestroyed };
                    }
                }
            }

            WorldObjectInstance worldObjectTarget = FindWorldObjectInView();

            if (worldObjectTarget is not null)
            {
                return AttackWorldObject(worldObjectTarget, weapon);
            }

            MobInstance target = FindTargetInView();

            if (target is null)
            {
                return new AttackResult { Outcome = AttackOutcome.NoTarget };
            }

            Mob mobDefinition = mobManager.GetMobDefinition(target.MobId);
            Player player = playerManager.GetPlayer();

            AttackOutcome outcome = ResolveHit(player, weapon, mobDefinition);

            if (outcome == AttackOutcome.Missed)
            {
                return new AttackResult { Outcome = AttackOutcome.Missed, MobName = mobDefinition.Name };
            }

            int damage = CalculateDamage(player, weapon, mobDefinition, outcome == AttackOutcome.Critical || outcome == AttackOutcome.CriticalKill);
            mobManager.ApplyDamageToMob(target, damage);

            int remainingAmmunition = 0;

            if (!string.IsNullOrEmpty(weapon.AmmunitionId))
            {
                player.AmmoCounts.TryGetValue(weapon.AmmunitionId, out remainingAmmunition);
            }

            if (target.CurrentHealth <= 0)
            {
                levelManager.RemoveMob(target.Id);
                mobManager.RemoveMob(target.Id);
                playerManager.AddExperience(mobDefinition.Health);

                AttackOutcome killOutcome = AttackOutcome.Kill;

                if (outcome == AttackOutcome.Critical)
                {
                    killOutcome = AttackOutcome.CriticalKill;
                }

                return new AttackResult
                {
                    Outcome = killOutcome,
                    Damage = damage,
                    MobName = mobDefinition.Name,
                    RemainingAmmunition = remainingAmmunition
                };
            }

            return new AttackResult
            {
                Outcome = outcome,
                Damage = damage,
                MobName = mobDefinition.Name,
                RemainingAmmunition = remainingAmmunition
            };
        }

        AttackResult AttackWorldObject(WorldObjectInstance worldObjectInstance, Weapon weapon)
        {
            WorldObject worldObjectDefinition = worldObjectDefinitions
                .FirstOrDefault(worldObject => worldObject.Id.Equals(worldObjectInstance.WorldObjectId));

            int damage = weapon.Damage;

            worldObjectInstance.CurrentHealth -= damage;

            Player player = playerManager.GetPlayer();

            int remainingAmmunition = 0;

            if (!string.IsNullOrEmpty(weapon.AmmunitionId))
            {
                player.AmmoCounts.TryGetValue(weapon.AmmunitionId, out remainingAmmunition);
            }

            if (worldObjectInstance.CurrentHealth > 0)
            {
                return new AttackResult
                {
                    Outcome = AttackOutcome.WorldObjectHit,
                    Damage = damage,
                    WorldObjectName = worldObjectDefinition.Name,
                    RemainingAmmunition = remainingAmmunition
                };
            }

            levelManager.RemoveWorldObject(worldObjectInstance.Id);

            int explosionDamageDealtToPlayer = 0;

            if (worldObjectDefinition.IsExplosive)
            {
                explosionDamageDealtToPlayer = TriggerExplosion(
                    worldObjectInstance.Position,
                    worldObjectDefinition);
            }

            return new AttackResult
            {
                Outcome = AttackOutcome.WorldObjectDestroyed,
                Damage = damage,
                WorldObjectName = worldObjectDefinition.Name,
                ExplosionDamageDealtToPlayer = explosionDamageDealtToPlayer,
                RemainingAmmunition = remainingAmmunition
            };
        }

        int TriggerExplosion(Point2D explosionPosition, WorldObject explosiveDefinition)
        {
            int explosionDamageDealtToPlayer = 0;

            Point2D[] adjacentPositions =
            [
                new Point2D(explosionPosition.X - 1, explosionPosition.Y),
                new Point2D(explosionPosition.X + 1, explosionPosition.Y),
                new Point2D(explosionPosition.X, explosionPosition.Y - 1),
                new Point2D(explosionPosition.X, explosionPosition.Y + 1),
                new Point2D(explosionPosition.X - 1, explosionPosition.Y - 1),
                new Point2D(explosionPosition.X + 1, explosionPosition.Y - 1),
                new Point2D(explosionPosition.X - 1, explosionPosition.Y + 1),
                new Point2D(explosionPosition.X + 1, explosionPosition.Y + 1)
            ];

            int explosionDamage = randomNumberGenerator.Next(
                explosiveDefinition.MinimumExplosionDamage,
                explosiveDefinition.MaximumExplosionDamage + 1);

            Player player = playerManager.GetPlayer();

            int playerTileX = (int)Math.Floor(player.Position.X);
            int playerTileY = (int)Math.Floor(player.Position.Y);

            foreach (Point2D adjacentPosition in adjacentPositions)
            {
                if (playerTileX == adjacentPosition.X && playerTileY == adjacentPosition.Y)
                {
                    playerManager.ApplyDamage(explosionDamage);
                    explosionDamageDealtToPlayer += explosionDamage;
                }

                foreach (MobInstance mobInstance in levelManager.GetMobs().Where(m => !m.IsFriendly).ToList())
                {
                    if (mobInstance.Position.X == adjacentPosition.X && mobInstance.Position.Y == adjacentPosition.Y)
                    {
                        mobManager.ApplyDamageToMob(mobInstance, explosionDamage);

                        if (mobInstance.CurrentHealth <= 0)
                        {
                            levelManager.RemoveMob(mobInstance.Id);
                            mobManager.RemoveMob(mobInstance.Id);
                        }
                    }
                }

                WorldObjectInstance adjacentWorldObject = levelManager.GetWorldObjectAtPosition(
                    adjacentPosition.X, adjacentPosition.Y);

                if (adjacentWorldObject is not null)
                {
                    WorldObject adjacentDefinition = worldObjectDefinitions
                        .FirstOrDefault(worldObject => worldObject.Id.Equals(adjacentWorldObject.WorldObjectId));

                    if (adjacentDefinition is not null)
                    {
                        if (adjacentDefinition.IsExplosive)
                        {
                            levelManager.RemoveWorldObject(adjacentWorldObject.Id);

                            explosionDamageDealtToPlayer += TriggerExplosion(
                                adjacentWorldObject.Position,
                                adjacentDefinition);
                        }
                        else if (adjacentDefinition.HealAmount == 0)
                        {
                            adjacentWorldObject.CurrentHealth -= explosionDamage;

                            if (adjacentWorldObject.CurrentHealth <= 0)
                            {
                                levelManager.RemoveWorldObject(adjacentWorldObject.Id);
                            }
                        }
                    }
                }
            }

            return explosionDamageDealtToPlayer;
        }

        MobInstance FindFirstMobInView()
        {
            Player player = playerManager.GetPlayer();

            float rayPositionX = player.Position.X;
            float rayPositionY = player.Position.Y;
            float directionX = player.Direction.X;
            float directionY = player.Direction.Y;

            float magnitude = (float)Math.Sqrt(directionX * directionX + directionY * directionY);

            if (magnitude < 0.0001f)
            {
                return null;
            }

            directionX /= magnitude;
            directionY /= magnitude;

            const float StepSize = 0.5f;
            const int MaxSteps = 20;

            for (int step = 1; step <= MaxSteps; step++)
            {
                float samplePositionX = rayPositionX + directionX * step * StepSize;
                float samplePositionY = rayPositionY + directionY * step * StepSize;

                int tileX = (int)Math.Floor(samplePositionX);
                int tileY = (int)Math.Floor(samplePositionY);

                if (levelManager.GetWall(tileX, tileY) is not null)
                {
                    break;
                }

                if (WorldObjectBlocksProjectilesAt(tileX, tileY))
                {
                    break;
                }

                MobInstance mob = levelManager
                    .GetMobs()
                    .FirstOrDefault(mobInstance => mobInstance.Position.X == tileX && mobInstance.Position.Y == tileY);

                if (mob is not null)
                {
                    return mob;
                }
            }

            return null;
        }

        WorldObjectInstance FindWorldObjectInView()
        {
            Player player = playerManager.GetPlayer();

            float rayPositionX = player.Position.X;
            float rayPositionY = player.Position.Y;
            float directionX = player.Direction.X;
            float directionY = player.Direction.Y;

            float magnitude = (float)Math.Sqrt(directionX * directionX + directionY * directionY);

            if (magnitude < 0.0001f)
            {
                return null;
            }

            directionX /= magnitude;
            directionY /= magnitude;

            const float StepSize = 0.5f;
            const int MaxSteps = 20;

            for (int step = 1; step <= MaxSteps; step++)
            {
                float samplePositionX = rayPositionX + directionX * step * StepSize;
                float samplePositionY = rayPositionY + directionY * step * StepSize;

                int tileX = (int)Math.Floor(samplePositionX);
                int tileY = (int)Math.Floor(samplePositionY);

                if (levelManager.GetWall(tileX, tileY) is not null)
                {
                    break;
                }

                bool tileOccupiedByMob = levelManager.GetMobs()
                    .Any(mob => mob.Position.X == tileX && mob.Position.Y == tileY);

                if (tileOccupiedByMob)
                {
                    break;
                }

                WorldObjectInstance worldObject = levelManager.GetWorldObjectAtPosition(tileX, tileY);

                if (worldObject is not null && WorldObjectBlocksProjectilesAt(tileX, tileY))
                {
                    return worldObject;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the nearest mob directly in the player's view using DDA ray marching.
        /// </summary>
        MobInstance FindTargetInView()
        {
            Player player = playerManager.GetPlayer();

            float rayPositionX = player.Position.X;
            float rayPositionY = player.Position.Y;
            float directionX = player.Direction.X;
            float directionY = player.Direction.Y;

            // Normalise direction.
            float magnitude = (float)Math.Sqrt(directionX * directionX + directionY * directionY);

            if (magnitude < 0.0001f)
            {
                return null;
            }

            directionX /= magnitude;
            directionY /= magnitude;

            const float StepSize = 0.5f;
            const int MaxSteps = 20;

            for (int step = 1; step <= MaxSteps; step++)
            {
                float samplePositionX = rayPositionX + directionX * step * StepSize;
                float samplePositionY = rayPositionY + directionY * step * StepSize;

                int tileX = (int)Math.Floor(samplePositionX);
                int tileY = (int)Math.Floor(samplePositionY);

                // Stop at walls.
                if (levelManager.GetWall(tileX, tileY) is not null)
                {
                    break;
                }

                // Stop at world objects that block projectiles.
                if (WorldObjectBlocksProjectilesAt(tileX, tileY))
                {
                    break;
                }

                MobInstance mob = levelManager
                    .GetMobs()
                    .FirstOrDefault(mobInstance => mobInstance.Position.X == tileX && mobInstance.Position.Y == tileY && !mobInstance.IsFriendly);

                if (mob is not null)
                {
                    return mob;
                }
            }

            return null;
        }

        bool WorldObjectBlocksProjectilesAt(int tileX, int tileY)
        {
            WorldObjectInstance worldObjectInstance = levelManager.GetWorldObjectAtPosition(tileX, tileY);

            if (worldObjectInstance is null)
            {
                return false;
            }

            WorldObject worldObjectDefinition = worldObjectDefinitions
                .FirstOrDefault(worldObject => worldObject.Id.Equals(worldObjectInstance.WorldObjectId));

            return worldObjectDefinition is not null && worldObjectDefinition.BlocksProjectiles;
        }

        /// <summary>
        /// Hit check formula derived from original game's u.a(attacker, weapon, defender, distance).
        /// Stats scaled to 0-255 fixed-point range where 128 = balanced (attacker == defender).
        /// </summary>
        AttackOutcome ResolveHit(Player player, Weapon weapon, Mob mob)
        {
            // Scale player accuracy to original 8-bit range (Accuracy 1 → 20, grows with upgrades)
            int attackerAccuracy = Math.Clamp(player.Accuracy * 20, 1, 255);

            // Derive mob evasion from its HP: squishier mobs are harder to track
            int defenderEvasion = Math.Clamp(200 - mob.Health / 2, 10, 200);

            // Original formula: hitChance = (attacker.acc * 128 / defender.evasion) + (weapon.d * 65536 / 51200)
            int hitChance = attackerAccuracy * 128 / defenderEvasion
                          + weapon.AccuracyBonus * 65536 / 51200;

            // Roll 0-255
            int roll = randomNumberGenerator.Next(256);

            if (roll >= hitChance)
            {
                return AttackOutcome.Missed;
            }

            // Crit threshold: hitChance * 8 / 5120 (rare at low levels, scales with accuracy investment)
            int criticalHitThreshold = hitChance * 8 / 5120;

            if (roll < criticalHitThreshold)
            {
                return AttackOutcome.Critical;
            }

            return AttackOutcome.Hit;
        }

        /// <summary>
        /// Damage formula derived from original game's u.a(attacker, weapon, defender, param, distance).
        /// </summary>
        int CalculateDamage(Player player, Weapon weapon, Mob mob, bool isCriticalHit)
        {
            // Weapon damage range ±15% (original had explicit min/max per weapon)
            int minimumDamage = weapon.Damage * 85 / 100;
            int maximumDamage = weapon.Damage * 115 / 100;

            // Random roll 0-255 to pick damage in range (original formula)
            int damageRoll = randomNumberGenerator.Next(256);
            int baseDamage = minimumDamage + damageRoll * (maximumDamage - minimumDamage) / 256;

            // Scale by attacker strength vs mob defence
            // Original: damage = base * (attacker.f / defender.e), both 0-255
            int attackerStrength = Math.Clamp(player.Strength * 20, 1, 255);
            int mobDefense = Math.Clamp(mob.Health / 5, 5, 200);

            int damage = baseDamage * attackerStrength / mobDefense;

            // Clamp to [1, 999] matching original game
            damage = Math.Clamp(damage, 1, 999);

            if (isCriticalHit)
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

        public DoorInteractionResult InteractWithDoor()
        {
            Player player = playerManager.GetPlayer();

            float directionX = player.Direction.X;
            float directionY = player.Direction.Y;
            float magnitude = (float)Math.Sqrt(directionX * directionX + directionY * directionY);

            if (magnitude < 0.0001f)
            {
                return DoorInteractionResult.NoDoor;
            }

            directionX /= magnitude;
            directionY /= magnitude;

            int tileX = (int)Math.Floor(player.Position.X + directionX);
            int tileY = (int)Math.Floor(player.Position.Y + directionY);

            WallInstance door = levelManager.GetDoorAtPosition(tileX, tileY);

            if (door is null)
            {
                return DoorInteractionResult.NoDoor;
            }

            Wall doorDefinition = wallDefinitions.FirstOrDefault(w => w.Id.Equals(door.WallId));

            if (doorDefinition is not null && !string.IsNullOrEmpty(doorDefinition.RequiredKeyId))
            {
                if (!playerManager.HasKey(doorDefinition.RequiredKeyId))
                {
                    return new DoorInteractionResult
                    {
                        ErrorMessage = $"Authorization required, but no {doorDefinition.Name.ToLower().Replace(" door", "")} key found."
                    };
                }
            }

            if (!string.IsNullOrEmpty(door.DestinationLevelId))
            {
                return new DoorInteractionResult { DestinationLevelId = door.DestinationLevelId };
            }

            door.IsOpen = !door.IsOpen;

            return new DoorInteractionResult();
        }

        public void ChangeLevel(string levelId)
        {
            levelManager.LoadContent(levelId);
            mobManager.LoadContent();

            Point2D spawn = levelManager.GetSpawnPosition();
            playerManager.SetPosition(spawn.X + 0.5f, spawn.Y + 0.5f);

            foreach (WorldObjectInstance worldObjectInstance in levelManager.GetWorldObjects())
            {
                WorldObject worldObjectDefinition = worldObjectDefinitions
                    .FirstOrDefault(worldObject => worldObject.Id.Equals(worldObjectInstance.WorldObjectId));

                if (worldObjectDefinition is not null)
                {
                    worldObjectInstance.CurrentHealth = worldObjectDefinition.Health;
                }
            }

            foreach (WallInstance wallInstance in levelManager.GetWalls())
            {
                Wall wallDefinition = wallDefinitions.FirstOrDefault(wall => wall.Id.Equals(wallInstance.WallId));

                if (wallDefinition is not null && wallDefinition.IsDoor)
                {
                    wallInstance.IsDoor = true;
                }
            }
        }

        public string InteractWithTerminal()
        {
            Player player = playerManager.GetPlayer();

            float directionX = player.Direction.X;
            float directionY = player.Direction.Y;
            float magnitude = (float)Math.Sqrt(directionX * directionX + directionY * directionY);

            if (magnitude < 0.0001f)
            {
                return null;
            }

            directionX /= magnitude;
            directionY /= magnitude;

            int tileX = (int)Math.Floor(player.Position.X + directionX);
            int tileY = (int)Math.Floor(player.Position.Y + directionY);

            TerminalInstance terminal = levelManager.GetTerminalAtPosition(tileX, tileY);

            if (terminal is null)
            {
                return null;
            }

            return terminal.Text;
        }

        public string InteractWithMob()
        {
            Player player = playerManager.GetPlayer();

            float directionX = player.Direction.X;
            float directionY = player.Direction.Y;
            float magnitude = (float)Math.Sqrt(directionX * directionX + directionY * directionY);

            if (magnitude < 0.0001f)
            {
                return null;
            }

            directionX /= magnitude;
            directionY /= magnitude;

            int tileX = (int)Math.Floor(player.Position.X + directionX);
            int tileY = (int)Math.Floor(player.Position.Y + directionY);

            MobInstance targetMob = levelManager.GetMobs()
                .FirstOrDefault(mobInstance =>
                    mobInstance.Position.X == tileX &&
                    mobInstance.Position.Y == tileY &&
                    mobInstance.IsFriendly &&
                    !string.IsNullOrEmpty(mobInstance.Dialogue));

            if (targetMob is null)
            {
                return null;
            }

            return targetMob.Dialogue;
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

        public IEnumerable<WorldObjectInstance> GetWorldObjectInstances()
            => levelManager.GetWorldObjects();

        public WorldObject GetWorldObjectDefinition(string id)
            => worldObjectDefinitions.FirstOrDefault(worldObject => worldObject.Id.Equals(id));
    }
}
