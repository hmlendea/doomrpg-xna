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
        internal static WallInstance ToDomainModel(this WallInstanceEntity wallInstanceEntity)
        {
            WallInstance wallInstance = new WallInstance
            {
                Id = wallInstanceEntity.Id,
                WallId = wallInstanceEntity.Id,
                Position = new Point2D(wallInstanceEntity.X, wallInstanceEntity.Y),
                IsRemovable = wallInstanceEntity.IsRemovable
            };

            return wallInstance;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="wallInstance">WallInstance.</param>
        internal static WallInstanceEntity ToEntity(this WallInstance wallInstance)
        {
            WallInstanceEntity wallInstanceEntity = new WallInstanceEntity
            {
                Id = wallInstance.Id,
                WallId = wallInstance.Id,
                X = wallInstance.Position.X,
                Y = wallInstance.Position.Y,
                IsRemovable = wallInstance.IsRemovable
            };

            return wallInstanceEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="wallInstanceEntities">WallInstance entities.</param>
        internal static IEnumerable<WallInstance> ToDomainModels(this IEnumerable<WallInstanceEntity> wallInstanceEntities)
        {
            IEnumerable<WallInstance> wallInstances = wallInstanceEntities.Select(wallInstanceEntity => wallInstanceEntity.ToDomainModel());

            return wallInstances;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="wallInstances">WallInstances.</param>
        internal static IEnumerable<WallInstanceEntity> ToEntities(this IEnumerable<WallInstance> wallInstances)
        {
            IEnumerable<WallInstanceEntity> wallInstanceEntities = wallInstances.Select(wallInstance => wallInstance.ToEntity());

            return wallInstanceEntities;
        }
    }
}
