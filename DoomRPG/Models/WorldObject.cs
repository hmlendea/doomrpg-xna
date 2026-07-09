namespace DoomRPG.Models
{
    public sealed class WorldObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SpritesheetName { get; set; }

        public int Health { get; set; }

        public bool IsExplosive { get; set; }

        public bool BlocksProjectiles { get; set; }

        public int MinimumExplosionDamage { get; set; }

        public int MaximumExplosionDamage { get; set; }

        public int HealAmount { get; set; }
    }
}
