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
        {
            WallInstance wall = currentLevel.Walls
                .FirstOrDefault(wallInstance => wallInstance.Position.X == x && wallInstance.Position.Y == y && !wallInstance.IsDestroyed);

            if (wall is not null && wall.IsDoor && wall.IsOpen)
            {
                return null;
            }

            return wall;
        }

        public WallInstance GetDoorAtPosition(int x, int y)
            => currentLevel.Walls.FirstOrDefault(wallInstance => wallInstance.Position.X == x && wallInstance.Position.Y == y && wallInstance.IsDoor);

        public IEnumerable<TerminalInstance> GetTerminals()
            => currentLevel.Terminals;

        public TerminalInstance GetTerminalAtPosition(int x, int y)
            => currentLevel.Terminals.FirstOrDefault(t => t.Position.X == x && t.Position.Y == y);

        public IEnumerable<WorldObjectInstance> GetWorldObjects()
            => currentLevel.WorldObjects.Where(worldObjectInstance => !worldObjectInstance.IsDestroyed);

        public WorldObjectInstance GetWorldObjectAtPosition(int x, int y)
            => currentLevel.WorldObjects.FirstOrDefault(worldObjectInstance =>
                worldObjectInstance.Position.X == x &&
                worldObjectInstance.Position.Y == y &&
                !worldObjectInstance.IsDestroyed);

        public void RemoveWorldObject(string worldObjectInstanceId)
        {
            WorldObjectInstance worldObjectInstance = currentLevel.WorldObjects
                .FirstOrDefault(instance => instance.Id.Equals(worldObjectInstanceId));

            if (worldObjectInstance is not null)
            {
                worldObjectInstance.IsDestroyed = true;
            }
        }

        public void RemoveWallAtPosition(int x, int y)
        {
            WallInstance wallInstance = currentLevel.Walls
                .FirstOrDefault(w => w.Position.X == x && w.Position.Y == y);

            if (wallInstance is not null)
            {
                wallInstance.IsDestroyed = true;
            }
        }

        public int GetTurnNumber()
            => currentLevel.TurnNumber;

        public void AdvanceTurn()
        {
            currentLevel.TurnNumber += 1;
        }
    }
}
