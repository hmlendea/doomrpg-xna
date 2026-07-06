namespace DoomRPG.Models
{
    public sealed class Weapon
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AmmunitionId { get; set; }

        public int Damage { get; set; }

        public int AmmoPerShot { get; set; }

        public string SpritesheetName { get; set; }

        public int SpritesheetTextureIndex { get; set; }
    }
}
