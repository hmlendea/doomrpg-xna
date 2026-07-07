using NuciXNA.Primitives;

namespace DoomRPG.Settings
{
    public sealed class GraphicsSettings
    {
        /// <summary>
        /// Gets or sets the resolution.
        /// </summary>
        /// <value>The resolution.</value>
        public Size2D Resolution { get; set; } = new(1024, 544);

        /// <summary>
        /// Gets or sets the fullscreen mode toggle.
        /// </summary>
        /// <value>The fullscreen mode.</value>
        public bool Fullscreen { get; set; }

        public bool FogOfWar { get; set; }

        public bool ShowRoofs { get; set; }
    }
}
