using System.Collections.Generic;
using System.Linq;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    static class WorldObjectMappingExtensions
    {
        internal static WorldObject ToDomainModel(this WorldObjectEntity worldObjectEntity) => new()
        {
            Id = worldObjectEntity.Id,
            Name = worldObjectEntity.Name,
            SpritesheetName = worldObjectEntity.SpritesheetName,
            SpritesheetTextureIndex = worldObjectEntity.SpritesheetTextureIndex,
            Health = worldObjectEntity.Health,
            IsExplosive = worldObjectEntity.IsExplosive,
            BlocksProjectiles = worldObjectEntity.BlocksProjectiles,
            BlocksMovement = worldObjectEntity.BlocksMovement,
            MinimumExplosionDamage = worldObjectEntity.MinimumExplosionDamage,
            MaximumExplosionDamage = worldObjectEntity.MaximumExplosionDamage,
            HealAmount = worldObjectEntity.HealAmount,
            WeaponId = worldObjectEntity.WeaponId,
            KeyId = worldObjectEntity.KeyId,
            AmmoId = worldObjectEntity.AmmoId,
            AmmoAmount = worldObjectEntity.AmmoAmount
        };

        internal static WorldObjectEntity ToDataObject(this WorldObject worldObject) => new()
        {
            Id = worldObject.Id,
            Name = worldObject.Name,
            SpritesheetName = worldObject.SpritesheetName,
            SpritesheetTextureIndex = worldObject.SpritesheetTextureIndex,
            Health = worldObject.Health,
            IsExplosive = worldObject.IsExplosive,
            BlocksProjectiles = worldObject.BlocksProjectiles,
            BlocksMovement = worldObject.BlocksMovement,
            MinimumExplosionDamage = worldObject.MinimumExplosionDamage,
            MaximumExplosionDamage = worldObject.MaximumExplosionDamage,
            HealAmount = worldObject.HealAmount,
            WeaponId = worldObject.WeaponId,
            KeyId = worldObject.KeyId,
            AmmoId = worldObject.AmmoId,
            AmmoAmount = worldObject.AmmoAmount
        };

        internal static IEnumerable<WorldObject> ToDomainModels(this IEnumerable<WorldObjectEntity> worldObjectEntities)
            => worldObjectEntities.Select(worldObjectEntity => worldObjectEntity.ToDomainModel());

        internal static IEnumerable<WorldObjectEntity> ToDataObjects(this IEnumerable<WorldObject> worldObjects)
            => worldObjects.Select(worldObject => worldObject.ToDataObject());
    }
}
