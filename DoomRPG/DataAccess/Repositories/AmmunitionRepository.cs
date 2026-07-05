using NuciDAL.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    /// <summary>
    /// Ammunition repository implementation.
    /// </summary>
    public sealed class AmmunitionRepository(string fileName) : XmlRepository<AmmunitionEntity>(fileName)
    {
    }
}
