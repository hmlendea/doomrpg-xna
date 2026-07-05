namespace DoomRPG.Models
{
    public sealed class Mob
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

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
        /// Gets or sets the spritsheet used to draw this wall.
        /// </summary>
        /// <value>The name of thr spritesheet.</value>
        public string SpritesheetName { get; set; }
    }
}
