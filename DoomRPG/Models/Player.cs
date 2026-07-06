using NuciXNA.Primitives;

namespace DoomRPG.Models
{
    public sealed class Player
    {
        public PointF2D Position { get; set; }

        // TODO: Change to Vector2D
        public PointF2D Direction { get; set; }

        public float MovementSpeed { get; set; }

        public int Health { get; set; }

        public int MaxHealth { get; set; }

        public int Armour { get; set; }

        public int MaxArmour { get; set; }

        public int Credits { get; set; }

        public bool IsAlive => Health > 0;

        public Player()
        {
            MovementSpeed = 0.075f;
        }
    }
}
