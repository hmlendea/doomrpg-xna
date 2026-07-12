using System;
using System.Collections.Generic;
using System.Linq;

using NuciXNA.Primitives;

using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Models;
using DoomRPG.Models.Enumerations;
using DoomRPG.Settings;

namespace DoomRPG.GameLogic.GameManagers
{
    public sealed class PlayerManager(ILevelManager levelManager) : IPlayerManager
    {
        readonly Player player = new()
        {
            Position = new PointF2D(3.5f, 4.5f),
            Health = GameDefines.PlayerStartingHealth,
            MaxHealth = GameDefines.PlayerStartingMaxHealth,
            Armour = GameDefines.PlayerStartingArmour,
            MaxArmour = GameDefines.PlayerStartingMaxArmour,
            Credits = GameDefines.PlayerStartingCredits,
            EquippedWeaponId = GameDefines.PlayerStartingWeaponId,
            Strength = GameDefines.PlayerStartingStrength,
            Agility = GameDefines.PlayerStartingAgility,
            Accuracy = GameDefines.PlayerStartingAccuracy,
            Defense = GameDefines.PlayerStartingDefense,
            StatPoints = GameDefines.PlayerStartingStatPoints,
            Level = GameDefines.PlayerStartingLevel,
            Experience = GameDefines.PlayerStartingExperience,
            AmmoCounts = new Dictionary<string, int>
            {
                { "bullet_clip", GameDefines.PlayerStartingBulletClips },
                { "shell_clip", 0 },
                { "rocket", 0 },
                { "cell_clip", 0 },
                { "halon_can", 0 }
            }
        };

        IEnumerable<string> weaponInventory = ["fist", "pistol"];

        public void LoadContent()
        {

        }

        public void UnloadContent()
        {

        }

        public void Update(float elapsedSeconds)
        {

        }

        public bool MovePlayer(MovementDirection direction)
        {
            int dirX = (int)Math.Round(player.Direction.X);
            int dirY = (int)Math.Round(player.Direction.Y);

            int dx = 0;
            int dy = 0;

            switch (direction)
            {
                case MovementDirection.North:
                    dx = dirX;
                    dy = dirY;
                    break;

                case MovementDirection.South:
                    dx = -dirX;
                    dy = -dirY;
                    break;

                case MovementDirection.West:
                    dx = dirY;
                    dy = -dirX;
                    break;

                case MovementDirection.East:
                    dx = -dirY;
                    dy = dirX;
                    break;
            }

            float targetX = player.Position.X + dx;
            float targetY = player.Position.Y + dy;

            WallInstance wall = levelManager.GetWall((int)targetX, (int)targetY);

            if (wall is not null)
            {
                return false;
            }

            bool tileOccupiedByMob = levelManager.GetMobs()
                .Any(mob => mob.Position.X == (int)targetX && mob.Position.Y == (int)targetY);

            if (tileOccupiedByMob)
            {
                return false;
            }

            player.Position = new PointF2D(targetX, targetY);

            return true;
        }

        public void RotatePlayer(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            player.Direction = new PointF2D(
                player.Direction.X * cos - player.Direction.Y * sin,
                player.Direction.X * sin + player.Direction.Y * cos);
        }

        public void ApplyDamage(int amount)
        {
            if (player.Armour > 0)
            {
                int absorbed = amount / 3;

                if (absorbed > player.Armour)
                {
                    absorbed = player.Armour;
                }

                player.Armour -= absorbed;
                amount -= absorbed;
            }

            player.Health -= amount;

            if (player.Health < 0)
            {
                player.Health = 0;
            }
        }

        public void Heal(int amount)
        {
            player.Health += amount;

            if (player.Health > player.MaxHealth)
            {
                player.Health = player.MaxHealth;
            }
        }

        public void RestoreArmour(int amount)
        {
            player.Armour += amount;

            if (player.Armour > player.MaxArmour)
            {
                player.Armour = player.MaxArmour;
            }
        }

        public void DepleteArmour(int amount)
        {
            player.Armour -= amount;

            if (player.Armour < 0)
            {
                player.Armour = 0;
            }
        }

        public void AddCredits(int amount)
        {
            player.Credits += amount;
        }

        public void AddExperience(int amount)
        {
            player.Experience += amount;

            while (player.Experience >= player.ExperienceToNextLevel)
            {
                player.Experience -= player.ExperienceToNextLevel;
                player.Level += 1;
                player.StatPoints += GameDefines.StatPointsPerLevel;
            }
        }

        public void GiveWeapon(string weaponId)
        {
            if (!weaponInventory.Contains(weaponId))
            {
                weaponInventory = [.. weaponInventory, weaponId];
            }
        }

        public void GiveKey(string keyId)
        {
            if (!player.Keys.Contains(keyId))
            {
                player.Keys.Add(keyId);
            }
        }

        public bool HasKey(string keyId)
            => player.Keys.Contains(keyId);

        public void SetPosition(float x, float y)
            => player.Position = new PointF2D(x, y);

        public bool SelectWeapon(string weaponId)
        {
            if (!weaponInventory.Contains(weaponId))
            {
                return false;
            }

            player.EquippedWeaponId = weaponId;

            return true;
        }

        public bool SelectWeaponBySlot(int slot)
        {
            if (slot < 1 || slot > weaponInventory.Count())
            {
                return false;
            }

            player.EquippedWeaponId = weaponInventory.ElementAt(slot - 1);

            return true;
        }

        public void CycleWeaponNext()
        {
            if (!weaponInventory.Any())
            {
                return;
            }

            int currentIndex = weaponInventory
                .Select((id, index) => (id, index))
                .First(x => x.id.Equals(player.EquippedWeaponId)).index;
            int nextIndex = (currentIndex + 1) % weaponInventory.Count();

            player.EquippedWeaponId = weaponInventory.ElementAt(nextIndex);
        }

        public void CycleWeaponPrevious()
        {
            if (!weaponInventory.Any())
            {
                return;
            }

            int currentIndex = weaponInventory
                .Select((id, index) => (id, index))
                .First(x => x.id.Equals(player.EquippedWeaponId)).index;
            int previousIndex = (currentIndex - 1 + weaponInventory.Count()) % weaponInventory.Count();

            player.EquippedWeaponId = weaponInventory.ElementAt(previousIndex);
        }

        public string GetEquippedWeaponId()
            => player.EquippedWeaponId;

        public IEnumerable<string> GetWeaponInventory()
            => weaponInventory;

        public bool AllocateStat(StatType stat)
        {
            if (player.StatPoints <= 0)
            {
                return false;
            }

            if (stat.Equals(StatType.Strength))
            {
                player.Strength += 1;
            }
            else if (stat.Equals(StatType.Agility))
            {
                player.Agility += 1;
            }
            else if (stat.Equals(StatType.Accuracy))
            {
                player.Accuracy += 1;
            }
            else if (stat.Equals(StatType.Defense))
            {
                player.Defense += 1;
            }

            player.StatPoints -= 1;

            return true;
        }

        public bool SpendCredits(int amount)
        {
            if (player.Credits < amount)
            {
                return false;
            }

            player.Credits -= amount;

            return true;
        }

        public void AddAmmo(string ammoId, int amount)
        {
            if (player.AmmoCounts.ContainsKey(ammoId))
            {
                player.AmmoCounts[ammoId] += amount;
            }
            else
            {
                player.AmmoCounts[ammoId] = amount;
            }
        }

        public bool SpendAmmo(string ammoId, int amount)
        {
            if (!player.AmmoCounts.TryGetValue(ammoId, out int current) || current < amount)
            {
                return false;
            }

            player.AmmoCounts[ammoId] -= amount;

            return true;
        }

        public Player GetPlayer()
            => player;
    }
}
