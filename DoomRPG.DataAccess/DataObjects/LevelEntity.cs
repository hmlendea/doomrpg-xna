using System.Collections.Generic;

using NuciXNA.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.DataObjects
{
    public sealed class LevelEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the ceiling colour in hexadecimal format.
        /// </summary>
        /// <value>The ceiling colour.</value>
        public string CeilingColourHex { get; set; }

        /// <summary>
        /// Gets or sets the floor colour in hexadecimal format.
        /// </summary>
        /// <value>The floor colour.</value>
        public string FloorColourHex { get; set; }

        public int SpawnX { get; set; }

        public int SpawnY { get; set; }

        public List<WallInstanceEntity> Walls { get; set; }
    }
}
