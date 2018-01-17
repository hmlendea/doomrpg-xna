using System.IO;
using System.Linq;

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
        /// <param name="id">Level identifier.</param>
        public void LoadContent(string id)
        {
            string levelPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "maps.xml");

            MapRepository levelRepository = new MapRepository(levelPath);

            currentLevel = levelRepository.Get(id).ToDomainModel();
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
