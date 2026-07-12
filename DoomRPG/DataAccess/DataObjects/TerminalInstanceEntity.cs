using NuciDAL.DataObjects;

namespace DoomRPG.DataAccess.DataObjects
{
    public sealed class TerminalInstanceEntity : EntityBase
    {
        public string Text { get; set; }

        public string SpritesheetName { get; set; }

        public int SpritesheetTextureIndex { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
