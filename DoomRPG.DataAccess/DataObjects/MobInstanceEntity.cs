using NuciXNA.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.DataObjects
{
    public sealed class MobInstanceEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the mob identifier.
        /// </summary>
        /// <value>The mob identifier.</value>
        public string MobId { get; set; }

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
        /// Gets or sets a value indicating whether this mob is friendly or not.
        /// Friendly mobs cannot be attacked.
        /// </summary>
        /// <value>True if the mob is friendly, false otherwise.</value>
        public bool IsFriendly { get; set; }
    }
}
