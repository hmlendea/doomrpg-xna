using NuciXNA.DataAccess;
using NuciXNA.DataAccess.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    /// <summary>
    /// Mob repository implementation.
    /// </summary>
    public sealed class MobRepository : XmlRepository<MobEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public MobRepository(string fileName) : base(fileName)
        {

        }

        /// <summary>
        /// Updates the specified mob.
        /// </summary>
        /// <param name="mobEntity">Mob.</param>
        public override void Update(MobEntity mobEntity)
        {
            LoadEntitiesIfNeeded();

            MobEntity mobEntityToUpdate = Get(mobEntity.Id);

            if (mobEntityToUpdate == null)
            {
                throw new EntityNotFoundException(mobEntity.Id, nameof(MobEntity));
            }

            mobEntityToUpdate.Name = mobEntity.Name;
            mobEntityToUpdate.Description = mobEntity.Description;
            mobEntityToUpdate.SpritesheetName = mobEntity.SpritesheetName;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
