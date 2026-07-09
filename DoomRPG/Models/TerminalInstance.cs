using NuciXNA.Primitives;

namespace DoomRPG.Models
{
    public sealed class TerminalInstance
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string SpritesheetName { get; set; }

        public int SpritesheetTextureIndex { get; set; }

        public Point2D Position { get; set; }
    }
}
