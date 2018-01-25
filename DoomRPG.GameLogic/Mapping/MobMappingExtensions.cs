using System.Collections.Generic;
using System.Linq;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    /// <summary>
    /// Mob mapping extensions for converting between entities and domain models.
    /// </summary>
    static class MobMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="mobEntity">Mob entity.</param>
        internal static Mob ToDomainModel(this MobEntity mobEntity)
        {
            Mob mob = new Mob
            {
                Id = mobEntity.Id,
                Name = mobEntity.Name,
                Description = mobEntity.Description,
                SpritesheetName = mobEntity.SpritesheetName
            };

            return mob;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="mob">Mob.</param>
        internal static MobEntity ToEntity(this Mob mob)
        {
            MobEntity mobEntity = new MobEntity
            {
                Id = mob.Id,
                Name = mob.Name,
                Description = mob.Description,
                SpritesheetName = mob.SpritesheetName
            };

            return mobEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="mobEntities">Mob entities.</param>
        internal static IEnumerable<Mob> ToDomainModels(this IEnumerable<MobEntity> mobEntities)
        {
            IEnumerable<Mob> mobs = mobEntities.Select(mobEntity => mobEntity.ToDomainModel());

            return mobs;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="mobs">Mobs.</param>
        internal static IEnumerable<MobEntity> ToEntities(this IEnumerable<Mob> mobs)
        {
            IEnumerable<MobEntity> mobEntities = mobs.Select(mob => mob.ToEntity());

            return mobEntities;
        }
    }
}
