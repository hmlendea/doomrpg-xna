using NuciDAL.Repositories;

using DoomRPG.DataAccess.DataObjects;

namespace DoomRPG.DataAccess.Repositories
{
    public sealed class MobClassRepository(string fileName) : XmlRepository<MobClassEntity>(fileName)
    {
    }
}
