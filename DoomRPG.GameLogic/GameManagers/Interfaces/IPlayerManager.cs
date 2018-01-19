using DoomRPG.Models;
using DoomRPG.Models.Enumerations;

namespace DoomRPG.GameLogic.GameManagers.Interfaces
{
    public interface IPlayerManager
    {
        void LoadContent();

        void UnloadContent();

        void Update(float elapsedSeconds);

        void SetPlayerMovementDirection(MovementDirection direction);

        Player GetPlayer();
    }
}
