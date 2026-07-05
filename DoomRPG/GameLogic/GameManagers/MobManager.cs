using System.Collections.Generic;
using System.IO;
using System.Linq;

using NuciXNA.DataAccess.Repositories;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.DataAccess.Repositories;
using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.GameLogic.Mapping;
using DoomRPG.Models;
using DoomRPG.Settings;

namespace DoomRPG.GameLogic.GameManagers
{
    public sealed class MobManager : IMobManager
    {
        readonly ILevelManager levelManager;

        Dictionary<string, Mob> mobDefinitions;
        Dictionary<string, MobInstance> mobInstances;

        public MobManager(ILevelManager levelManager)
        {
            this.levelManager = levelManager;
        }

        public void LoadContent()
        {
            string mobsPath = Path.Combine(ApplicationPaths.EntitiesDirectory, "mobs.xml");

            IRepository<string, MobEntity> mobsRepository = new MobRepository(mobsPath);

            mobDefinitions = new Dictionary<string, Mob>();

            mobDefinitions = mobsRepository.GetAll().ToDomainModels().ToDictionary(x => x.Id, x => x);
            mobInstances = levelManager.GetMobs().ToDictionary(x => x.Id, x => x);
        }

        public void UnloadContent()
        {
            mobDefinitions.Clear();
        }

        public void Update(float elapsedSeconds)
        {

        }

        public Mob GetMobDefinition(string mobDefinitionId)
            => mobDefinitions[mobDefinitionId];

        public IEnumerable<Mob> GetMobDefinitions()
            => mobDefinitions.Values;

        public MobInstance GetMobInstance(string mobInstanceId)
            => mobInstances[mobInstanceId];

        public IEnumerable<MobInstance> GetMobInstances()
            => mobInstances.Values;
    }
}
