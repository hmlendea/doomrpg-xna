using System.Collections.Generic;
using System.Linq;

using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    /// <summary>
    /// Level mapping extensions for converting between entities and domain models.
    /// </summary>
    static class LevelMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="levelEntity">Level entity.</param>
        internal static Level ToDomainModel(this LevelEntity levelEntity)
        {
            Level level = new Level
            {
                Name = levelEntity.Name,
                Description = levelEntity.Description,
                Size = new Size2D(levelEntity.Width, levelEntity.Height),
                CeilingColour = ColourTranslator.FromHexadecimal(levelEntity.CeilingColourHex),
                FloorColour = ColourTranslator.FromHexadecimal(levelEntity.FloorColourHex),
                SpawnPosition = new Point2D(levelEntity.SpawnX, levelEntity.SpawnY),
                Walls = levelEntity.Walls.ToList().ToDomainModels(),
                Mobs = levelEntity.Mobs.ToList().ToDomainModels()
            };

            return level;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="level">Level.</param>
        internal static LevelEntity ToEntity(this Level level)
        {
            LevelEntity levelEntity = new LevelEntity
            {
                Name = level.Name,
                Description = level.Description,
                Width = level.Size.Width,
                Height = level.Size.Height,
                CeilingColourHex = level.CeilingColour.ToHexadecimal(),
                FloorColourHex = level.FloorColour.ToHexadecimal(),
                SpawnX = level.SpawnPosition.X,
                SpawnY = level.SpawnPosition.Y,
                Walls = level.Walls.ToEntities().ToList(),
                Mobs = level.Mobs.ToEntities().ToList()
            };

            return levelEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="levelEntities">Level entities.</param>
        internal static IEnumerable<Level> ToDomainModels(this IEnumerable<LevelEntity> levelEntities)
        {
            IEnumerable<Level> levels = levelEntities.Select(levelEntity => levelEntity.ToDomainModel());

            return levels;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="levels">Levels.</param>
        internal static IEnumerable<LevelEntity> ToEntities(this IEnumerable<Level> levels)
        {
            IEnumerable<LevelEntity> levelEntities = levels.Select(level => level.ToEntity());

            return levelEntities;
        }
    }
}
