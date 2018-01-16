using NuciXNA.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.DataObjects
{
    public sealed class WallInstanceEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the wall identifier.
        /// </summary>
        /// <value>The wall identifier.</value>
        public string WallId { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>The X coordinate.</value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        /// <value>The Y coordinate.</value>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this wall is removable or not.
        /// Removable walls are used to hide secret areas, and are removed by the player when activating them.
        /// </summary>
        /// <value>True if the wall is removable, false otherwise.</value>
        public bool IsRemovable { get; set; }
    }
}
