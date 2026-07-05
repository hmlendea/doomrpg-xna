using System.Collections.Generic;

using DoomRPG.Models;

namespace DoomRPG.GameLogic.GameManagers.Interfaces
{
    public interface IMobManager
    {
        void LoadContent();

        void UnloadContent();

        void Update(float elapsedSeconds);

        Mob GetMobDefinition(string mobDefinitionId);

        IEnumerable<Mob> GetMobDefinitions();

        MobInstance GetMobInstance(string mobInstanceId);

        IEnumerable<MobInstance> GetMobInstances();
    }
}
