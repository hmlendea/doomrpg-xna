using System.Collections.Generic;
using System.Linq;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    static class WeaponMappingExtensions
    {
        internal static Weapon ToDomainModel(this WeaponEntity weaponEntity) => new()
        {
            Id = weaponEntity.Id,
            Name = weaponEntity.Name,
            Description = weaponEntity.Description,
            AmmunitionId = weaponEntity.AmmunitionId,
            Damage = weaponEntity.Damage,
            AmmoPerShot = weaponEntity.AmmoPerShot,
            SpritesheetName = weaponEntity.SpritesheetName,
            SpritesheetTextureIndex = weaponEntity.SpritesheetTextureIndex
        };

        internal static WeaponEntity ToDataObject(this Weapon weapon) => new()
        {
            Id = weapon.Id,
            Name = weapon.Name,
            Description = weapon.Description,
            AmmunitionId = weapon.AmmunitionId,
            Damage = weapon.Damage,
            AmmoPerShot = weapon.AmmoPerShot,
            SpritesheetName = weapon.SpritesheetName,
            SpritesheetTextureIndex = weapon.SpritesheetTextureIndex
        };

        internal static IEnumerable<Weapon> ToDomainModels(this IEnumerable<WeaponEntity> weaponEntities)
            => weaponEntities.Select(weaponEntity => weaponEntity.ToDomainModel());
    }
}
