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

        void RotatePlayer(float angle);

        void ApplyDamage(int amount);

        void Heal(int amount);

        void RestoreArmour(int amount);

        void DepleteArmour(int amount);

        void AddCredits(int amount);

        bool SpendCredits(int amount);

        void AddExperience(int amount);

        bool AllocateStat(StatType stat);

        Player GetPlayer();
    }
}
