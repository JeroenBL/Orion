using System.Data;

namespace Orion.Services
{
    public interface IDataAccessService
    {
        IDbConnection LoadConnectionString();
    }
}