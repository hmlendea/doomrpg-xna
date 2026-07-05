using System.Collections.Generic;

using NuciXNA.Primitives;

using DoomRPG.Models;
using DoomRPG.Models.Enumerations;

namespace DoomRPG.GameLogic.GameManagers.Interfaces
{
    public interface IGameManager
    {
        void LoadContent();

        void UnloadContent();

        void Update(float elapsedSeconds);

        void MovePlayer(MovementDirection direction);

        void RotatePlayer(float angle);

        Size2D GetLevelSize();

        Colour GetLevelCeilingColour();

        Colour GetLevelFloorColour();

        IEnumerable<Wall> GetLevelWallDefinitions();

        IEnumerable<WallInstance> GetWalls();

        Wall GetWallDefinition(string id);

        WallInstance GetWall(int x, int y);

        Player GetPlayer();
    }
}
