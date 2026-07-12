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

        MobClass GetMobClassDefinition(string mobClassId);

        IEnumerable<MobClass> GetMobClassDefinitions();

        MobInstance GetMobInstance(string mobInstanceId);

        IEnumerable<MobInstance> GetMobInstances();

        void InitialiseMobHealth(MobInstance mobInstance);

        void ApplyDamageToMob(MobInstance mobInstance, int damage);

        void RemoveMob(string mobInstanceId);
    }
}
