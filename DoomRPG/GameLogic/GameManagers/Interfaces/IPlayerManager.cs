using System.Collections.Generic;

using DoomRPG.Models;
using DoomRPG.Models.Enumerations;

namespace DoomRPG.GameLogic.GameManagers.Interfaces
{
    public interface IPlayerManager
    {
        void LoadContent();

        void UnloadContent();

        void Update(float elapsedSeconds);

        bool MovePlayer(MovementDirection direction);

        void RotatePlayer(float angle);

        void ApplyDamage(int amount);

        void Heal(int amount);

        void RestoreArmour(int amount);

        void DepleteArmour(int amount);

        void AddCredits(int amount);

        bool SpendCredits(int amount);

        void GiveWeapon(string weaponId);

        bool SelectWeapon(string weaponId);

        bool SelectWeaponBySlot(int slot);

        void CycleWeaponNext();

        void CycleWeaponPrevious();

        string GetEquippedWeaponId();

        IEnumerable<string> GetWeaponInventory();

        void AddAmmo(string ammoId, int amount);

        bool SpendAmmo(string ammoId, int amount);

        void AddExperience(int amount);

        bool AllocateStat(StatType stat);

        Player GetPlayer();
    }
}
