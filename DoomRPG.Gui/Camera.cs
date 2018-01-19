using Microsoft.Xna.Framework;

using DoomRPG.Models;

namespace DoomRPG.Gui
{
    public class Camera
    {
        Player player;

        public Vector2 Position { get; set; }
        
        public Vector2 Direction { get; set; }
        
        public Vector2 Plane { get; set; }

        public float Radius { get; set; }

        public float RotationSpeed { get; set; }

        public Camera()
        {
            Radius = 0.3f;
            RotationSpeed = 3f;
        }
        
        public void Update(GameTime gameTime)
        {
            Direction = new Vector2(player.Direction.X, player.Direction.Y);
            Position = new Vector2(player.Position.X, player.Position.Y);
        }

        public void AssociatePlayer(Player player)
        {
            this.player = player;
        }
    }
}
