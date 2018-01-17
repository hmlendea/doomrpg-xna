using DoomRPG.Models;

namespace DoomRPG.GameLogic.GameManagers.Interfaces
{
    public interface ILevelManager
    {
        /// <summary>
        /// Gets the wall.
        /// </summary>
        /// <returns>The wall.</returns>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        WallInstance GetWall(int x, int y);
    }
}
