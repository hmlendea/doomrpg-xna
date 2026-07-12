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
        internal static MobInstance ToDomainModel(this MobInstanceEntity mobInstanceEntity) => new()
        {
            Id = mobInstanceEntity.Id,
            MobId = mobInstanceEntity.MobId,
            Position = new Point2D(mobInstanceEntity.X, mobInstanceEntity.Y),
            IsFriendly = mobInstanceEntity.IsFriendly,
            Dialogue = mobInstanceEntity.Dialogue
        };

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="mobInstance">MobInstance.</param>
        internal static MobInstanceEntity ToDataObject(this MobInstance mobInstance) => new()
        {
            Id = mobInstance.Id,
            MobId = mobInstance.MobId,
            X = mobInstance.Position.X,
            Y = mobInstance.Position.Y,
            IsFriendly = mobInstance.IsFriendly,
            Dialogue = mobInstance.Dialogue
        };

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="mobInstanceEntities">MobInstance entities.</param>
        internal static IEnumerable<MobInstance> ToDomainModels(this IEnumerable<MobInstanceEntity> mobInstanceEntities)
            => mobInstanceEntities.Select(mobInstanceEntity => mobInstanceEntity.ToDomainModel());

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="mobInstances">MobInstances.</param>
        internal static IEnumerable<MobInstanceEntity> ToDataObjects(this IEnumerable<MobInstance> mobInstances)
            => mobInstances.Select(mobInstance => mobInstance.ToDataObject());
    }
}
