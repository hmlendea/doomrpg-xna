using System.Linq;

using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    /// <summary>
    /// Map repository implementation.
    /// </summary>
    public class MapRepository : XmlRepository<MapEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public MapRepository(string fileName) : base(fileName)
        {

        }

        /// <summary>
        /// Updates the specified map.
        /// </summary>
        /// <param name="mapEntity">Map.</param>
        public override void Update(MapEntity mapEntity)
        {
            LoadEntitiesIfNeeded();

            MapEntity mapEntityToUpdate = Entities.FirstOrDefault(x => x.Id == mapEntity.Id);

            if (mapEntityToUpdate == null)
            {
                throw new EntityNotFoundException(mapEntity.Id, nameof(MapEntity));
            }

            mapEntityToUpdate.Name = mapEntity.Name;
            mapEntityToUpdate.Description = mapEntity.Description;
            mapEntityToUpdate.Width = mapEntity.Width;
            mapEntityToUpdate.Height = mapEntity.Height;
            mapEntityToUpdate.CeilingColourHex = mapEntity.CeilingColourHex;
            mapEntityToUpdate.FloorColourHex = mapEntity.FloorColourHex;

            XmlFile.SaveEntities(Entities);
        }
    }
}
