using NuciDAL.DataObjects;

namespace DoomRPG.DataAccess.DataObjects
{
    public sealed class WorldObjectEntity : EntityBase
    {
        public string Name { get; set; }

        public string SpritesheetName { get; set; }

        public int SpritesheetTextureIndex { get; set; }

        public int Health { get; set; }

        public bool IsExplosive { get; set; }

        public bool BlocksProjectiles { get; set; }

        public bool BlocksMovement { get; set; }

        public int MinimumExplosionDamage { get; set; }

        public int MaximumExplosionDamage { get; set; }

        public int HealAmount { get; set; }

        public int ArmourAmount { get; set; }

        public string WeaponId { get; set; }

        public string KeyId { get; set; }

        public string AmmoId { get; set; }

        public int AmmoAmount { get; set; }

        public int DamageOnContact { get; set; }

        public bool IsExtinguishable { get; set; }
    }
}
