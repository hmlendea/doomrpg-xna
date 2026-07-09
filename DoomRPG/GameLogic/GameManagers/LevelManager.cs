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
    public sealed class LevelManager : ILevelManager
    {
        Level currentLevel;

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="levelId">Level identifier.</param>
        public void LoadContent(string levelId)
        {
            string levelPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "levels.xml");

            LevelRepository levelRepository = new(levelPath);

            currentLevel = levelRepository.Get(levelId).ToDomainModel();
        }

        public void UnloadContent()
        {

        }

        public void Update(float elapsedSeconds)
        {

        }

        public Size2D GetSize()
            => currentLevel.Size;

        public Colour GetCeilingColour()
            => currentLevel.CeilingColour;

        public Colour GetFloorColour()
            => currentLevel.FloorColour;

        public IEnumerable<WallInstance> GetWalls()
            => currentLevel.Walls;

        public IEnumerable<MobInstance> GetMobs()
            => currentLevel.Mobs;

        public void RemoveMob(string mobInstanceId)
            => currentLevel.Mobs = [.. currentLevel.Mobs.Where(m => m.Id != mobInstanceId)];

        /// <summary>
        /// Gets the wall.
        /// </summary>
        /// <returns>The wall.</returns>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public WallInstance GetWall(int x, int y)
            => currentLevel.Walls.FirstOrDefault(wall => wall.Position.X == x && wall.Position.Y == y);

        public IEnumerable<TerminalInstance> GetTerminals()
            => currentLevel.Terminals;

        public TerminalInstance GetTerminalAtPosition(int x, int y)
            => currentLevel.Terminals.FirstOrDefault(t => t.Position.X == x && t.Position.Y == y);

        public int GetTurnNumber()
            => currentLevel.TurnNumber;

        public void AdvanceTurn()
        {
            currentLevel.TurnNumber += 1;
        }
    }
}
