using System.Linq;

using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    /// <summary>
    /// Ammunition repository implementation.
    /// </summary>
    public class AmmunitionRepository : XmlRepository<AmmunitionEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmmunitionRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public AmmunitionRepository(string fileName) : base(fileName)
        {

        }

        /// <summary>
        /// Updates the specified ammunition.
        /// </summary>
        /// <param name="ammunitionEntity">Ammunition.</param>
        public override void Update(AmmunitionEntity ammunitionEntity)
        {
            LoadEntitiesIfNeeded();

            AmmunitionEntity ammunitionEntityToUpdate = Entities.FirstOrDefault(x => x.Id == ammunitionEntity.Id);

            if (ammunitionEntityToUpdate == null)
            {
                throw new EntityNotFoundException(ammunitionEntity.Id, nameof(AmmunitionEntity));
            }

            ammunitionEntityToUpdate.Name = ammunitionEntity.Name;
            ammunitionEntityToUpdate.Description = ammunitionEntity.Description;
            ammunitionEntityToUpdate.SpritesheetName = ammunitionEntity.SpritesheetName;
            ammunitionEntityToUpdate.SpritesheetTextureIndex = ammunitionEntity.SpritesheetTextureIndex;

            XmlFile.SaveEntities(Entities);
        }
    }
}
