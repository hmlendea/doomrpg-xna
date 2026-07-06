using NuciDAL.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    /// <summary>
    /// Wall repository implementation.
    /// </summary>
    public sealed class WallRepository(string fileName) : XmlRepository<WallEntity>(fileName)
    {
    }
}
