using NuciDAL.DataObjects;

namespace DoomRPG.DataAccess.DataObjects
{
    public sealed class WeaponEntity : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string AmmunitionId { get; set; }

        public int Damage { get; set; }

        public int AmmoPerShot { get; set; }

        public string SpritesheetName { get; set; }

        public int SpritesheetTextureIndex { get; set; }
    }
}
