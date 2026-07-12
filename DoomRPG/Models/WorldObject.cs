namespace DoomRPG.Models
{
    public sealed class WorldObject
    {
        public string Id { get; set; }

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

        public string WeaponId { get; set; }

        public string KeyId { get; set; }
    }
}
