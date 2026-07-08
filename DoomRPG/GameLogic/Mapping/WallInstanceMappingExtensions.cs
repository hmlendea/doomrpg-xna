using System.Collections.Generic;
using System.Linq;

using NuciXNA.Primitives;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    /// <summary>
    /// WallInstance mapping extensions for converting between entities and domain models.
    /// </summary>
    static class WallInstanceMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="wallInstanceEntity">WallInstance entity.</param>
        internal static WallInstance ToDomainModel(this WallInstanceEntity wallInstanceEntity) => new()
        {
            Id = wallInstanceEntity.Id,
            WallId = wallInstanceEntity.WallId,
            Position = new Point2D(wallInstanceEntity.X, wallInstanceEntity.Y),
            IsRemovable = wallInstanceEntity.IsRemovable
        };

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="wallInstance">WallInstance.</param>
        internal static WallInstanceEntity ToDataObject(this WallInstance wallInstance) => new()
        {
            Id = wallInstance.Id,
            WallId = wallInstance.WallId,
            X = wallInstance.Position.X,
            Y = wallInstance.Position.Y,
            IsRemovable = wallInstance.IsRemovable
        };

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="wallInstanceEntities">WallInstance entities.</param>
        internal static IEnumerable<WallInstance> ToDomainModels(this IEnumerable<WallInstanceEntity> wallInstanceEntities)
            => wallInstanceEntities.Select(wallInstanceEntity => wallInstanceEntity.ToDomainModel());

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="wallInstances">WallInstances.</param>
        internal static IEnumerable<WallInstanceEntity> ToDataObjects(this IEnumerable<WallInstance> wallInstances)
            => wallInstances.Select(wallInstance => wallInstance.ToDataObject());
    }
}
