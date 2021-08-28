using Dapper;
using Orion.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Orion.Services
{
    public class DataAccessService : IDataAccessService
    {
        public IDbConnection LoadConnectionString()
        {
            var dllLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dbPath = Path.Combine(dllLocation);
            IDbConnection connection = new SQLiteConnection($"Data Source={dbPath}\\orion.db");

            return connection;
        }
    }
}
