﻿using System.Linq;

using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    /// <summary>
    /// Wall repository implementation.
    /// </summary>
    public class WallRepository : XmlRepository<WallEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WallRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public WallRepository(string fileName) : base(fileName)
        {

        }

        /// <summary>
        /// Updates the specified wall.
        /// </summary>
        /// <param name="wallEntity">Wall.</param>
        public override void Update(WallEntity wallEntity)
        {
            LoadEntitiesIfNeeded();

            WallEntity wallEntityToUpdate = Entities.FirstOrDefault(x => x.Id == wallEntity.Id);

            if (wallEntityToUpdate == null)
            {
                throw new EntityNotFoundException(wallEntity.Id, nameof(WallEntity));
            }

            wallEntityToUpdate.Name = wallEntity.Name;
            wallEntityToUpdate.Description = wallEntity.Description;
            wallEntityToUpdate.Texture = wallEntity.Texture;

            XmlFile.SaveEntities(Entities);
        }
    }
}