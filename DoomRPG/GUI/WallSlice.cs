namespace DoomRPG.Gui
{
    internal struct WallSlice
    {
        public double Depth { get; internal set; }

        public int Height { get; internal set; }

        public int TextureX { get; internal set; }

        public string Spritesheet { get; internal set; }

        public int SpritesheetTextureIndex { get; internal set; }
    }
}
