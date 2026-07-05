using NuciDAL.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    /// <summary>
    /// Mob repository implementation.
    /// </summary>
    public sealed class MobRepository(string fileName) : XmlRepository<MobEntity>(fileName)
    {
    }
}
