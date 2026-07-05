using NuciXNA.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.DataObjects
{
    public sealed class AmmunitionEntity : EntityBase
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
        /// Gets or sets the spritsheet used to draw this wall.
        /// </summary>
        /// <value>The name of thr spritesheet.</value>
        public string SpritesheetName { get; set; }

        /// <summary>
        /// Gets or sets the texture index of this wall.
        /// </summary>
        /// <value>The texture index.</value>
        public int SpritesheetTextureIndex { get; set; }
    }
}
