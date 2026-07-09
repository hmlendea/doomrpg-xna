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

        AttackResult Attack();

        void RotatePlayer(float angle);

        Size2D GetLevelSize();

        Colour GetLevelCeilingColour();

        Colour GetLevelFloorColour();

        IEnumerable<Wall> GetLevelWallDefinitions();

        IEnumerable<WallInstance> GetWalls();

        Wall GetWallDefinition(string id);

        WallInstance GetWall(int x, int y);

        IEnumerable<TerminalInstance> GetTerminalInstances();

        TerminalInstance GetTerminalAtPosition(int x, int y);

        string InteractWithTerminal();

        IEnumerable<Weapon> GetWeaponDefinitions();

        Weapon GetWeaponDefinition(string id);

        IEnumerable<Ammunition> GetAmmunitionDefinitions();

        void AddAmmo(string ammoId, int amount);

        bool SpendAmmo(string ammoId, int amount);

        void GiveWeapon(string weaponId);

        bool SelectWeapon(string weaponId);

        bool SelectWeaponBySlot(int slot);

        void CycleWeaponNext();

        void CycleWeaponPrevious();

        Weapon GetEquippedWeapon();

        void AddExperience(int amount);

        bool AllocateStat(StatType stat);

        int GetTurnNumber();

        Player GetPlayer();

        IEnumerable<MobInstance> GetMobInstances();

        Mob GetMobDefinition(string id);
    }
}
