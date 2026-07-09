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
            Health = worldObjectEntity.Health,
            IsExplosive = worldObjectEntity.IsExplosive,
            MinimumExplosionDamage = worldObjectEntity.MinimumExplosionDamage,
            MaximumExplosionDamage = worldObjectEntity.MaximumExplosionDamage,
            HealAmount = worldObjectEntity.HealAmount
        };

        internal static WorldObjectEntity ToDataObject(this WorldObject worldObject) => new()
        {
            Id = worldObject.Id,
            Name = worldObject.Name,
            SpritesheetName = worldObject.SpritesheetName,
            Health = worldObject.Health,
            IsExplosive = worldObject.IsExplosive,
            MinimumExplosionDamage = worldObject.MinimumExplosionDamage,
            MaximumExplosionDamage = worldObject.MaximumExplosionDamage,
            HealAmount = worldObject.HealAmount
        };

        internal static IEnumerable<WorldObject> ToDomainModels(this IEnumerable<WorldObjectEntity> worldObjectEntities)
            => worldObjectEntities.Select(worldObjectEntity => worldObjectEntity.ToDomainModel());

        internal static IEnumerable<WorldObjectEntity> ToDataObjects(this IEnumerable<WorldObject> worldObjects)
            => worldObjects.Select(worldObject => worldObject.ToDataObject());
    }
}
