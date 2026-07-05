using NuciDAL.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    /// <summary>
    /// Level repository implementation.
    /// </summary>
    public sealed class LevelRepository(string fileName) : XmlRepository<LevelEntity>(fileName)
    {
    }
}
