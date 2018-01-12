using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Primitives;

namespace DoomRPG.Graphics
{
    public class Sprite
    {
        public Point2D Position { get; set; }

        public Texture2D Texture { get; private set; }

        public Sprite(Point2D position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
        }
    }
}
