using System.Collections.Generic;
using System.IO;
using System.Linq;

using NuciXNA.Primitives;

using DoomRPG.DataAccess.Repositories;
using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.GameLogic.Mapping;
using DoomRPG.Models;
using DoomRPG.Settings;

namespace DoomRPG.GameLogic.GameManagers
{
    public class LevelManager : ILevelManager
    {
        Map currentLevel;

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="levelId">Level identifier.</param>
        public void LoadContent(string levelId)
        {
            string levelPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "maps.xml");

            MapRepository levelRepository = new MapRepository(levelPath);

            currentLevel = levelRepository.Get(levelId).ToDomainModel();
        }

        public void UnloadContent()
        {

        }

        public void Update(float elapsedSeconds)
        {

        }

        public Size2D GetSize()
        {
            return currentLevel.Size;
        }

        public IEnumerable<WallInstance> GetWalls()
        {
            return currentLevel.Walls;
        }

        /// <summary>
        /// Gets the wall.
        /// </summary>
        /// <returns>The wall.</returns>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public WallInstance GetWall(int x, int y)
        {
            return currentLevel.Walls.FirstOrDefault(wall => wall.Position.X == x && wall.Position.Y == y);
        }
    }
}
