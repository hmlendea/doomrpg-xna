using System.Collections.Generic;
using System.IO;
using System.Linq;

using NuciDAL.Repositories;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.DataAccess.Repositories;
using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.GameLogic.Mapping;
using DoomRPG.Models;
using DoomRPG.Settings;

namespace DoomRPG.GameLogic.GameManagers
{
    public sealed class MobManager(ILevelManager levelManager) : IMobManager
    {
        Dictionary<string, MobClass> mobClassDefinitions;
        Dictionary<string, Mob> mobDefinitions;
        Dictionary<string, MobInstance> mobInstances;

        public void LoadContent()
        {
            string mobClassesPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "mob-classes.xml");
            string mobsPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "mobs.xml");

            MobClassRepository mobClassRepository = new(mobClassesPath);
            IRepository<string, MobEntity> mobsRepository = new MobRepository(mobsPath);

            mobClassDefinitions = mobClassRepository.GetAll().ToDomainModels().ToDictionary(x => x.Id, x => x);
            mobDefinitions = mobsRepository.GetAll().ToDomainModels().ToDictionary(x => x.Id, x => x);
            mobInstances = levelManager.GetMobs().ToDictionary(x => x.Id, x => x);

            foreach (MobInstance mobInstance in mobInstances.Values)
            {
                InitialiseMobHealth(mobInstance);
            }
        }

        public void UnloadContent()
        {
            mobClassDefinitions.Clear();
            mobDefinitions.Clear();
        }

        public void Update(float elapsedSeconds)
        {

        }

        public Mob GetMobDefinition(string mobDefinitionId)
            => mobDefinitions[mobDefinitionId];

        public IEnumerable<Mob> GetMobDefinitions()
            => mobDefinitions.Values;

        public MobClass GetMobClassDefinition(string mobClassId)
            => mobClassDefinitions[mobClassId];

        public IEnumerable<MobClass> GetMobClassDefinitions()
            => mobClassDefinitions.Values;

        public MobInstance GetMobInstance(string mobInstanceId)
            => mobInstances[mobInstanceId];

        public IEnumerable<MobInstance> GetMobInstances()
            => mobInstances.Values;

        public void InitialiseMobHealth(MobInstance mobInstance)
        {
            if (mobDefinitions.TryGetValue(mobInstance.MobId, out Mob mob))
            {
                mobInstance.CurrentHealth = mob.Health;
            }
        }

        public void ApplyDamageToMob(MobInstance mobInstance, int damage)
        {
            mobInstance.CurrentHealth -= damage;

            if (mobInstance.CurrentHealth < 0)
            {
                mobInstance.CurrentHealth = 0;
            }
        }

        public void RemoveMob(string mobInstanceId)
            => mobInstances.Remove(mobInstanceId);
    }
}
