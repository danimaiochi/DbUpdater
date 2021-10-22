using System;
using System.Threading.Tasks;
using DbUpdater.Helpers;

namespace DbUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var config = ConfigurationHelper.LoadAndValidateConfigs();
                var dbUpdater = new DbUpdater(config);

                await dbUpdater.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}