using NuciXNA.Primitives;

namespace DoomRPG.Models
{
    public sealed class WallInstance
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the wall identifier.
        /// </summary>
        /// <value>The wall identifier.</value>
        public string WallId { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Point2D Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this wall is removable or not.
        /// Removable walls are used to hide secret areas, and are removed by the player when activating them.
        /// </summary>
        /// <value>True if the wall is removable, false otherwise.</value>
        public bool IsRemovable { get; set; }

        public I
    }
}
