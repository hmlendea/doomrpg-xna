using System.Collections.Generic;
using System.Linq;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    /// <summary>
    /// Wall mapping extensions for converting between entities and domain models.
    /// </summary>
    static class WallMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="wallEntity">Wall entity.</param>
        internal static Wall ToDomainModel(this WallEntity wallEntity)
        {
            Wall wall = new Wall
            {
                Name = wallEntity.Name,
                Description = wallEntity.Description,
                SpritesheetName = wallEntity.SpritesheetName,
                SpritesheetTextureIndex = wallEntity.SpritesheetTextureIndex
            };

            return wall;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="wall">Wall.</param>
        internal static WallEntity ToEntity(this Wall wall)
        {
            WallEntity wallEntity = new WallEntity
            {
                Name = wall.Name,
                Description = wall.Description,
                SpritesheetName = wall.SpritesheetName,
                SpritesheetTextureIndex = wall.SpritesheetTextureIndex
            };

            return wallEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="wallEntities">Wall entities.</param>
        internal static IEnumerable<Wall> ToDomainModels(this IEnumerable<WallEntity> wallEntities)
        {
            IEnumerable<Wall> walls = wallEntities.Select(wallEntity => wallEntity.ToDomainModel());

            return walls;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="walls">Walls.</param>
        internal static IEnumerable<WallEntity> ToEntities(this IEnumerable<Wall> walls)
        {
            IEnumerable<WallEntity> wallEntities = walls.Select(wall => wall.ToEntity());

            return wallEntities;
        }
    }
}
