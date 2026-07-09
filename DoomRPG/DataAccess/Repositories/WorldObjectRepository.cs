using NuciDAL.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    public sealed class WorldObjectRepository(string fileName) : XmlRepository<WorldObjectEntity>(fileName)
    {
    }
}
