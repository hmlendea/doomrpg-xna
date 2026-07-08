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

        public bool Attack()
        {
            Weapon weapon = GetEquippedWeapon();

            if (weapon is null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(weapon.AmmunitionId) && weapon.AmmoPerShot > 0)
            {
                bool ammoSpent = playerManager.SpendAmmo(weapon.AmmunitionId, weapon.AmmoPerShot);

                if (!ammoSpent)
                {
                    return false;
                }
            }

            levelManager.AdvanceTurn();

            return true;
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
