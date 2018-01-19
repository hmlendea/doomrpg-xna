using Microsoft.Xna.Framework;
using NuciXNA.Primitives;

namespace DoomRPG.Gui
{
    public class Camera
    {
        public PointF2D Position { get; set; }

        public Size2D Size { get; set; }

        // TODO: Change to Vector2D
        public PointF2D Direction { get; set; }

        // TODO: Change to Vector2D
        public PointF2D Plane { get; set; }

        public float Radius { get; set; }

        public float RotationSpeed { get; set; }

        public Camera()
        {
            Radius = 0.3f;

            RotationSpeed = 3f;
        }
        
        public void Update(GameTime gameTime)
        {

        }
    }
}
