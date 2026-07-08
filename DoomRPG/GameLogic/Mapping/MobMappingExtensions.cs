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
        internal static Mob ToDomainModel(this MobEntity mobEntity) => new()
        {
            Id = mobEntity.Id,
            Name = mobEntity.Name,
            Description = mobEntity.Description,
            SpritesheetName = mobEntity.SpritesheetName,
            ClassId = mobEntity.ClassId,
            Health = mobEntity.Health,
            Damage = mobEntity.Damage
        };

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="mob">Mob.</param>
        internal static MobEntity ToDataObject(this Mob mob) => new()
        {
            Id = mob.Id,
            Name = mob.Name,
            Description = mob.Description,
            SpritesheetName = mob.SpritesheetName,
            ClassId = mob.ClassId,
            Health = mob.Health,
            Damage = mob.Damage
        };

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="mobEntities">Mob entities.</param>
        internal static IEnumerable<Mob> ToDomainModels(this IEnumerable<MobEntity> mobEntities)
            => mobEntities.Select(mobEntity => mobEntity.ToDomainModel());

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="mobs">Mobs.</param>
        internal static IEnumerable<MobEntity> ToDataObjects(this IEnumerable<Mob> mobs)
            => mobs.Select(mob => mob.ToDataObject());
    }
}
