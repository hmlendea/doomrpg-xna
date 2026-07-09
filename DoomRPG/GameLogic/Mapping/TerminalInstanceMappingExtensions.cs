using System.Collections.Generic;
using System.Linq;

using NuciXNA.Primitives;

using DoomRPG.DataAccess.DataObjects;
using DoomRPG.Models;

namespace DoomRPG.GameLogic.Mapping
{
    static class TerminalInstanceMappingExtensions
    {
        internal static TerminalInstance ToDomainModel(this TerminalInstanceEntity entity) => new()
        {
            Id = entity.Id,
            Text = entity.Text,
            SpritesheetName = entity.SpritesheetName,
            SpritesheetTextureIndex = entity.SpritesheetTextureIndex,
            Position = new Point2D(entity.X, entity.Y)
        };

        internal static TerminalInstanceEntity ToDataObject(this TerminalInstance terminal) => new()
        {
            Id = terminal.Id,
            Text = terminal.Text,
            SpritesheetName = terminal.SpritesheetName,
            SpritesheetTextureIndex = terminal.SpritesheetTextureIndex,
            X = terminal.Position.X,
            Y = terminal.Position.Y
        };

        internal static IEnumerable<TerminalInstance> ToDomainModels(this IEnumerable<TerminalInstanceEntity> terminalInstanceEntities)
            => terminalInstanceEntities.Select(terminalInstanceEntity => terminalInstanceEntity.ToDomainModel());

        internal static IEnumerable<TerminalInstanceEntity> ToDataObjects(this IEnumerable<TerminalInstance> terminalInstances)
            => terminalInstances.Select(terminalInstance => terminalInstance.ToDataObject());
    }
}
