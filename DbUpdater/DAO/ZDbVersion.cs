using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace DbUpdater.DAO
{
    public class ZDbVersion
    {
        private readonly IDbConnection _dbConnection;

        public ZDbVersion(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<string> Get()
        {
            var sql = "SELECT version FROM Z_DB_VERSION";
            using (_dbConnection)
            {
                var version = await _dbConnection.QueryFirstAsync<string>(sql);

                return version;
            }
        }

        private async Task Update(string version)
        {
            var sql = "UPDATE Z_DB_VERSION SET VERSION = @version";
            using (_dbConnection)
            {
                await _dbConnection.ExecuteScalarAsync(sql, new {version});
            }
        }

        public async Task Update(Version version)
        {
            await Update(version.ToString());
        }

        public async Task ExecuteSql(string sql)
        {
            using (_dbConnection)
            {
                await _dbConnection.ExecuteScalarAsync(sql);
            }
        }
    }
}