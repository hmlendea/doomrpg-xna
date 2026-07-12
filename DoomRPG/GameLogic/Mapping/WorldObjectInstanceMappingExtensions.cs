using System.Collections.Generic;
using System.Linq;

using NuciXNA.Primitives;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    static class WorldObjectInstanceMappingExtensions
    {
        internal static WorldObjectInstance ToDomainModel(this WorldObjectInstanceEntity worldObjectInstanceEntity) => new()
        {
            Id = worldObjectInstanceEntity.Id,
            WorldObjectId = worldObjectInstanceEntity.WorldObjectId,
            Position = new Point2D(worldObjectInstanceEntity.X, worldObjectInstanceEntity.Y)
        };

        internal static WorldObjectInstanceEntity ToDataObject(this WorldObjectInstance worldObjectInstance) => new()
        {
            Id = worldObjectInstance.Id,
            WorldObjectId = worldObjectInstance.WorldObjectId,
            X = worldObjectInstance.Position.X,
            Y = worldObjectInstance.Position.Y
        };

        internal static IEnumerable<WorldObjectInstance> ToDomainModels(this IEnumerable<WorldObjectInstanceEntity> worldObjectInstanceEntities)
            => worldObjectInstanceEntities.Select(worldObjectInstanceEntity => worldObjectInstanceEntity.ToDomainModel());

        internal static IEnumerable<WorldObjectInstanceEntity> ToDataObjects(this IEnumerable<WorldObjectInstance> worldObjectInstances)
            => worldObjectInstances.Select(worldObjectInstance => worldObjectInstance.ToDataObject());
    }
}
