using System.Collections.Generic;
using System.Linq;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    static class MobClassMappingExtensions
    {
        internal static MobClass ToDomainModel(this MobClassEntity entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };

        internal static MobClassEntity ToDataObject(this MobClass mobClass) => new()
        {
            Id = mobClass.Id,
            Name = mobClass.Name,
            Description = mobClass.Description
        };

        internal static IEnumerable<MobClass> ToDomainModels(this IEnumerable<MobClassEntity> entities)
            => entities.Select(e => e.ToDomainModel());
    }
}
