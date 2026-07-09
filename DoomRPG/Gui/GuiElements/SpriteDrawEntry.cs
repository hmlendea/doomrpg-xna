using Microsoft.Xna.Framework.Graphics;

namespace DoomRPG.Gui.GuiElements
{
    public sealed class SpriteDrawEntry
    {
        public Texture2D Texture { get; set; }

        public double PositionX { get; set; }

        public double PositionY { get; set; }

        public double SquaredDistance { get; set; }
    }
}
