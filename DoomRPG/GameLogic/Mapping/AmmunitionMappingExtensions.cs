using System.Collections.Generic;
using System.Linq;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    /// <summary>
    /// Ammunition mapping extensions for converting between entities and domain models.
    /// </summary>
    static class AmmunitionMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="ammunitionEntity">Ammunition entity.</param>
        internal static Ammunition ToDomainModel(this AmmunitionEntity ammunitionEntity)
        {
            Ammunition ammunition = new Ammunition
            {
                Id = ammunitionEntity.Id,
                Name = ammunitionEntity.Name,
                Description = ammunitionEntity.Description,
                SpritesheetName = ammunitionEntity.SpritesheetName
            };

            return ammunition;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="ammunition">Ammunition.</param>
        internal static AmmunitionEntity ToEntity(this Ammunition ammunition)
        {
            AmmunitionEntity ammunitionEntity = new AmmunitionEntity
            {
                Id = ammunition.Id,
                Name = ammunition.Name,
                Description = ammunition.Description,
                SpritesheetName = ammunition.SpritesheetName
            };

            return ammunitionEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="ammunitionEntities">Ammunition entities.</param>
        internal static IEnumerable<Ammunition> ToDomainModels(this IEnumerable<AmmunitionEntity> ammunitionEntities)
        {
            IEnumerable<Ammunition> ammunitions = ammunitionEntities.Select(ammunitionEntity => ammunitionEntity.ToDomainModel());

            return ammunitions;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="ammunitions">Ammunitions.</param>
        internal static IEnumerable<AmmunitionEntity> ToEntities(this IEnumerable<Ammunition> ammunitions)
        {
            IEnumerable<AmmunitionEntity> ammunitionEntities = ammunitions.Select(ammunition => ammunition.ToEntity());

            return ammunitionEntities;
        }
    }
}
