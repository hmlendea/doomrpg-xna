using System.Collections.Generic;
using System.Linq;

using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    /// <summary>
    /// Map mapping extensions for converting between entities and domain models.
    /// </summary>
    static class MapMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="mapEntity">Map entity.</param>
        internal static Map ToDomainModel(this MapEntity mapEntity)
        {
            Map map = new Map
            {
                Name = mapEntity.Name,
                Description = mapEntity.Description,
                Size = new Size2D(mapEntity.Width, mapEntity.Height),
                CeilingColour = ColourTranslator.FromHexadecimal(mapEntity.CeilingColourHex),
                FloorColour = ColourTranslator.FromHexadecimal(mapEntity.FloorColourHex),
                SpawnPosition = new Point2D(mapEntity.SpawnX, mapEntity.SpawnY),
                Walls = mapEntity.Walls.ToList().ToDomainModels()
            };

            return map;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="map">Map.</param>
        internal static MapEntity ToEntity(this Map map)
        {
            MapEntity mapEntity = new MapEntity
            {
                Name = map.Name,
                Description = map.Description,
                Width = map.Size.Width,
                Height = map.Size.Height,
                CeilingColourHex = map.CeilingColour.ToHexadecimal(),
                FloorColourHex = map.FloorColour.ToHexadecimal(),
                SpawnX = map.SpawnPosition.X,
                SpawnY = map.SpawnPosition.Y,
                Walls = map.Walls.ToEntities().ToList()
            };

            return mapEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="mapEntities">Map entities.</param>
        internal static IEnumerable<Map> ToDomainModels(this IEnumerable<MapEntity> mapEntities)
        {
            IEnumerable<Map> maps = mapEntities.Select(mapEntity => mapEntity.ToDomainModel());

            return maps;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="maps">Maps.</param>
        internal static IEnumerable<MapEntity> ToEntities(this IEnumerable<Map> maps)
        {
            IEnumerable<MapEntity> mapEntities = maps.Select(map => map.ToEntity());

            return mapEntities;
        }
    }
}
