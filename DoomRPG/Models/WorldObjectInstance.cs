using NuciXNA.Primitives;

namespace DoomRPG.Models
{
    public sealed class WorldObjectInstance
    {
        public string Id { get; set; }

        public string WorldObjectId { get; set; }

        public Point2D Position { get; set; }

        public int CurrentHealth { get; set; }

        public bool IsDestroyed { get; set; }
    }
}
