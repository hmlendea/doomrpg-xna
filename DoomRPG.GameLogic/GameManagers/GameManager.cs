using System.Collections.Generic;
using System.IO;
using System.Linq;

using DoomRPG.DataAccess.Repositories;
using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.GameLogic.Mapping;
using DoomRPG.Models;
using DoomRPG.Models.Enumerations;
using DoomRPG.Settings;

namespace DoomRPG.GameLogic.GameManagers
{
    public class GameManager : IGameManager
    {
        List<Wall> wallDefinitions;

        readonly ILevelManager levelManager;
        readonly IPlayerManager playerManager;

        public GameManager()
        {
            levelManager = new LevelManager();
            playerManager = new PlayerManager(levelManager);
        }

        public void LoadContent()
        {
            levelManager.LoadContent("test"); // TODO: Remove hardcoding
            playerManager.LoadContent();

            string wallPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "walls.xml");
            WallRepository wallRepository = new WallRepository(wallPath);

            wallDefinitions = wallRepository.GetAll().ToDomainModels().ToList();
        }

        public void UnloadContent()
        {
            levelManager.UnloadContent();
            playerManager.UnloadContent();

            wallDefinitions.Clear();
        }

        public void Update(float elapsedSeconds)
        {
            levelManager.Update(elapsedSeconds);
            playerManager.Update(elapsedSeconds);
        }

        public void MovePlayer(MovementDirection direction)
        {
            playerManager.SetPlayerMovementDirection(direction);
        }

        public IEnumerable<Wall> GetLevelWallDefinitions()
        {
            IEnumerable<WallInstance> walls = levelManager.GetWalls();

            return wallDefinitions.Where(wallDefinition => walls.Any(wall => wall.WallId == wallDefinition.Id));
        }

        public IEnumerable<WallInstance> GetWalls()
        {
            return levelManager.GetWalls();
        }

        public Wall GetWallDefinition(string id)
        {
            return wallDefinitions.FirstOrDefault(wall => wall.Id == id);
        }

        public WallInstance GetWall(int x, int y)
        {
            return levelManager.GetWall(x, y);
        }

        public Player GetPlayer()
        {
            return playerManager.GetPlayer();
        }
    }
}
