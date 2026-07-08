using NuciDAL.DataObjects;

namespace DoomRPG.DataAccess.DataObjects
{
    public sealed class MobClassEntity : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
