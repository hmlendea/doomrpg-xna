using System.Collections.Generic;

using DoomRPG.Models;

namespace DoomRPG.GameLogic.GameManagers.Interfaces
{
    public interface ILevelManager
    {
        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="levelId">Level identifier.</param>
        void LoadContent(string levelId);

        void UnloadContent();

        void Update(float elapsedSeconds);

        IEnumerable<WallInstance> GetWalls();

        /// <summary>
        /// Gets the wall.
        /// </summary>
        /// <returns>The wall.</returns>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        WallInstance GetWall(int x, int y);
    }
}
