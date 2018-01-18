using NuciXNA.Primitives;

using DoomRPG.Models.Enumerations;

namespace DoomRPG.Models
{
    public sealed class Player
    {
        public PointF2D Position { get; set; }

        // TODO: Change to Vector2D
        public PointF2D Direction { get; set; }
        
        public float MovementSpeed { get; set; }

        public MovementDirection MovementDirection { get; set; }

        public Player()
        {
            MovementSpeed = 3f;
        }
    }
}
