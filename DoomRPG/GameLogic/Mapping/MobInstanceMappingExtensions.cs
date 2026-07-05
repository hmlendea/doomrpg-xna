using System.Collections.Generic;
using System.Linq;

using NuciXNA.Primitives;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    /// <summary>
    /// MobInstance mapping extensions for converting between entities and domain models.
    /// </summary>
    static class MobInstanceMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="mobInstanceEntity">MobInstance entity.</param>
        internal static MobInstance ToDomainModel(this MobInstanceEntity mobInstanceEntity)
        {
            MobInstance mobInstance = new MobInstance
            {
                Id = mobInstanceEntity.Id,
                MobId = mobInstanceEntity.MobId,
                Position = new Point2D(mobInstanceEntity.X, mobInstanceEntity.Y),
                IsFriendly = mobInstanceEntity.IsFriendly
            };

            return mobInstance;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="mobInstance">MobInstance.</param>
        internal static MobInstanceEntity ToEntity(this MobInstance mobInstance)
        {
            MobInstanceEntity mobInstanceEntity = new MobInstanceEntity
            {
                Id = mobInstance.Id,
                MobId = mobInstance.MobId,
                X = mobInstance.Position.X,
                Y = mobInstance.Position.Y,
                IsFriendly = mobInstance.IsFriendly
            };

            return mobInstanceEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="mobInstanceEntities">MobInstance entities.</param>
        internal static IEnumerable<MobInstance> ToDomainModels(this IEnumerable<MobInstanceEntity> mobInstanceEntities)
        {
            IEnumerable<MobInstance> mobInstances = mobInstanceEntities.Select(mobInstanceEntity => mobInstanceEntity.ToDomainModel());

            return mobInstances;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="mobInstances">MobInstances.</param>
        internal static IEnumerable<MobInstanceEntity> ToEntities(this IEnumerable<MobInstance> mobInstances)
        {
            IEnumerable<MobInstanceEntity> mobInstanceEntities = mobInstances.Select(mobInstance => mobInstance.ToEntity());

            return mobInstanceEntities;
        }
    }
}
