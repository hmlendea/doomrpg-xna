using NuciDAL.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    public sealed class WeaponRepository(string fileName) : XmlRepository<WeaponEntity>(fileName)
    {
    }
}
