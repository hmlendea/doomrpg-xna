using NuciXNA.Primitives;

namespace DoomRPG.Models
{
    public sealed class MobInstance
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the mob identifier.
        /// </summary>
        /// <value>The mob identifier.</value>
        public string MobId { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Point2D Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this mob is friendly or not.
        /// Friendly mobs cannot be attacked.
        /// </summary>
        /// <value>True if the mob is friendly, false otherwise.</value>
        public bool IsFriendly { get; set; }
    }
}
