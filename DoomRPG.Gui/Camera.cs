using Microsoft.Xna.Framework;

namespace DoomRPG.Gui
{
    public class Camera
    {
        public Vector2 Position { get; set; }

        public Vector2 Direction { get; set; }

        public float Radius { get; set; }

        public float MovementSpeed { get; set; }

        public float RotationSpeed { get; set; }

        public Camera()
        {
            Radius = 0.3f;

            MovementSpeed = 3f;
            RotationSpeed = 3f;
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
