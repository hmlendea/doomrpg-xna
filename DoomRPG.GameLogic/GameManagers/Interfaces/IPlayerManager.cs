using DoomRPG.Models;
using DoomRPG.Models.Enumerations;

namespace DoomRPG.GameLogic.GameManagers.Interfaces
{
    public interface IPlayerManager
    {
        void LoadContent();

        void UnloadContent();

        void Update(float elapsedSeconds);

        void MovePlayer(MovementDirection direction);

        Player GetPlayer();
    }
}
