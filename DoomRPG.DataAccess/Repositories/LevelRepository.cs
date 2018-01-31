using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    /// <summary>
    /// Level repository implementation.
    /// </summary>
    public class LevelRepository : XmlRepository<LevelEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public LevelRepository(string fileName) : base(fileName)
        {

        }

        /// <summary>
        /// Updates the specified level.
        /// </summary>
        /// <param name="levelEntity">Level.</param>
        public override void Update(LevelEntity levelEntity)
        {
            LoadEntitiesIfNeeded();

            LevelEntity levelEntityToUpdate = Get(levelEntity.Id);

            if (levelEntityToUpdate == null)
            {
                throw new EntityNotFoundException(levelEntity.Id, nameof(LevelEntity));
            }

            levelEntityToUpdate.Name = levelEntity.Name;
            levelEntityToUpdate.Description = levelEntity.Description;
            levelEntityToUpdate.Width = levelEntity.Width;
            levelEntityToUpdate.Height = levelEntity.Height;
            levelEntityToUpdate.CeilingColourHex = levelEntity.CeilingColourHex;
            levelEntityToUpdate.FloorColourHex = levelEntity.FloorColourHex;
            levelEntityToUpdate.SpawnX = levelEntity.SpawnX;
            levelEntityToUpdate.SpawnY = levelEntity.SpawnY;
            levelEntityToUpdate.Walls = levelEntity.Walls;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
