using NuciXNA.Primitives;

namespace DoomRPG.Models
{
    public sealed class Map
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
        /// Gets or sets the ceiling colour.
        /// </summary>
        /// <value>The ceiling colour.</value>
        public Colour CeilingColour { get; set; }

        /// <summary>
        /// Gets or sets the floor colour.
        /// </summary>
        /// <value>The floor colour.</value>
        public Colour FloorColour { get; set; }
    }
}
