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
            levelManager.LoadContent("test"); // TODO: Remove hardcoding
            mobManager.LoadContent();
            playerManager.LoadContent();

            string ammoPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "ammo.xml");
            string wallPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "walls.xml");

            string weaponPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "weapons.xml");

            AmmunitionRepository ammoRepository = new AmmunitionRepository(ammoPath);
            WallRepository wallRepository = new WallRepository(wallPath);
            WeaponRepository weaponRepository = new WeaponRepository(weaponPath);

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

        public void RotatePlayer(float angle)
        {
            playerManager.RotatePlayer(angle);
        }

        public Size2D GetLevelSize()
        {
            return levelManager.GetSize();
        }

        public int GetTurnNumber()
        {
            return levelManager.GetTurnNumber();
        }

        public Colour GetLevelCeilingColour()
        {
            return levelManager.GetCeilingColour();
        }

        public Colour GetLevelFloorColour()
        {
            return levelManager.GetFloorColour();
        }

        public IEnumerable<Wall> GetLevelWallDefinitions()
        {
            IEnumerable<WallInstance> walls = levelManager.GetWalls();

            return wallDefinitions.Where(wallDefinition => walls.Any(wall => wall.WallId.Equals(wallDefinition.Id)));
        }

        public IEnumerable<WallInstance> GetWalls()
        {
            return levelManager.GetWalls();
        }

        public Wall GetWallDefinition(string id)
        {
            return wallDefinitions.FirstOrDefault(wall => wall.Id.Equals(id));
        }

        public WallInstance GetWall(int x, int y)
        {
            return levelManager.GetWall(x, y);
        }

        public IEnumerable<Weapon> GetWeaponDefinitions()
        {
            return weaponDefinitions;
        }

        public Weapon GetWeaponDefinition(string id)
        {
            return weaponDefinitions.FirstOrDefault(weapon => weapon.Id.Equals(id));
        }

        public IEnumerable<Ammunition> GetAmmunitionDefinitions()
        {
            return ammunitions;
        }

        public void AddAmmo(string ammoId, int amount)
        {
            playerManager.AddAmmo(ammoId, amount);
        }

        public bool SpendAmmo(string ammoId, int amount)
        {
            return playerManager.SpendAmmo(ammoId, amount);
        }

        public void GiveWeapon(string weaponId)
        {
            playerManager.GiveWeapon(weaponId);
        }

        public bool SelectWeapon(string weaponId)
        {
            return playerManager.SelectWeapon(weaponId);
        }

        public bool SelectWeaponBySlot(int slot)
        {
            return playerManager.SelectWeaponBySlot(slot);
        }

        public void CycleWeaponNext()
        {
            playerManager.CycleWeaponNext();
        }

        public void CycleWeaponPrevious()
        {
            playerManager.CycleWeaponPrevious();
        }

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
        {
            playerManager.AddExperience(amount);
        }

        public bool AllocateStat(StatType stat)
        {
            return playerManager.AllocateStat(stat);
        }

        public Player GetPlayer()
        {
            return playerManager.GetPlayer();
        }
    }
}
