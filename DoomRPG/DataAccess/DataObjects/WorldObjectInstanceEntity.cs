using NuciDAL.DataObjects;

namespace DoomRPG.DataAccess.DataObjects
{
    public sealed class WorldObjectInstanceEntity : EntityBase
    {
        public string WorldObjectId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
