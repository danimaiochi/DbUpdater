using System;
using System.IO;
using System.Threading.Tasks;
using DbUpdater.DAO;
using DbUpdater.Entities;
using DbUpdater.Helpers;
using MySql.Data.MySqlClient;

namespace DbUpdater
{
    public class DbUpdater
    {
        private ZDbVersion _zDbVersionDao;

        private readonly Config _config;

        public DbUpdater(Config config)
        {
            _config = config;
        }
        public async Task Run()
        {
            var sqlConnection = new MySqlConnection(_config.ConnectionString);
            _zDbVersionDao = new ZDbVersion(sqlConnection);

            var version = await GetDbVersion();

            var pendingFiles = FilesHelper.GetPendingUpdateFiles(version);

            foreach (var pendingFile in pendingFiles)
            {
                var contents = await File.ReadAllTextAsync(pendingFile.filePath);

                try
                {
                    await ExecuteSql(contents);
                }
                catch (Exception e)
                {
                    throw new Exception($"There was a problem when trying to execute the file {pendingFile.filePath}. {e.Message}");
                }

                try
                {
                    await UpdateDbVersion(pendingFile.version);
                }
                catch (Exception e)
                {
                    throw new Exception($"There was a problem when updating Z_DB_VERSION to {pendingFile.version}. {e.Message}");
                }
            }
        }

        private async Task<Version> GetDbVersion()
        {
            try
            {
                var version = await _zDbVersionDao.Get();

                return new Version(version);
            }
            catch (Exception e)
            {
                throw new Exception($@"There was a problem while retrieving the current DB Version." +
                                    $"\nMake sure you have a table called Z_DB_VERSION with only one record in a version format ('2021.10')." +
                                    $"\n{e.Message}");
            }
        }

        private async Task UpdateDbVersion(Version version)
        {
            try
            {
                await _zDbVersionDao.Update(version);
            }
            catch (Exception e)
            {
                throw new Exception($"There was a problem updating the Z_DB_VERSION to version {version}." +
                                    $"\n{e.Message}");
            }
        }

        private async Task ExecuteSql(string sql)
        {
            await _zDbVersionDao.ExecuteSql(sql);
        }
    }
}