using NuciDAL.DataObjects;

namespace DoomRPG.DataAccess.DataObjects
{
    public sealed class WorldObjectEntity : EntityBase
    {
        public string Name { get; set; }

        public string SpritesheetName { get; set; }

        public int Health { get; set; }

        public bool IsExplosive { get; set; }

        public int MinimumExplosionDamage { get; set; }

        public int MaximumExplosionDamage { get; set; }

        public int HealAmount { get; set; }
    }
}
