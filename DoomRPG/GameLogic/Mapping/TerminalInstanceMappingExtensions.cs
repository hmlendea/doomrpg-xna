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

        internal static IEnumerable<TerminalInstance> ToDomainModels(this IEnumerable<TerminalInstanceEntity> entities)
            => entities.Select(e => e.ToDomainModel());

        internal static IEnumerable<TerminalInstanceEntity> ToDataObjects(this IEnumerable<TerminalInstance> terminals)
            => terminals.Select(t => t.ToDataObject());
    }
}
