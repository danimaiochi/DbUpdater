using System;
using DbUpdater.Entities;
using Microsoft.Extensions.Configuration;

namespace DbUpdater.Helpers
{
    public class ConfigurationHelper
    {
        public static Config LoadAndValidateConfigs()
        {
            var config = Load();
            //Validate(config);

            return config;
        }

        private static Config Load()
        {
            var config = new Config();
            var configRoot = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true);

            ConfigurationBinder.Bind(configRoot, config);

            return config;
        }

        /*
        private static void Validate(Config config)
        {
            var errorFound = false;
            Log.Information($"Config - QueryLimit: {config.QueryLimit}");
            Log.Information($"Config - QueryOnlyInactiveAccounts: {config.QueryOnlyInactiveAccounts}");
            Log.Information($"Config - DaysInactive: {config.DaysInactive}");
            Log.Information($"Config - DryRun: {config.DryRun}");
            Log.Information($"Config - ConnectionString: {config.ConnectionString}");
            Log.Information($"Config - AccountId: {config.AccountId}");
            if (config.QueryLimit < 0)
            {
                Log.Error("Config - QueryLimit should be greater than 0");
                errorFound = true;
            }
            if (config.QueryLimit == 0)
            {
                Log.Information($"Config - QueryLimit: is 0, will NOT limit the query");
            }

            if (config.DaysInactive < 200)
            {
                Log.Error("Config - DaysInactive should be at least 200");
                errorFound = true;
            }

            if (string.IsNullOrEmpty(config.ConnectionString))
            {
                Log.Error("Config - ConnectionString cannot be empty");
                errorFound = true;
            }

            if (!config.AccountId.HasValue)
            {
                Log.Information("Config - AccountId is empty, no accountId filter will be made");
            }

            if (errorFound)
            {
                Log.Error("Config - There was one or more problems with the config, exiting...");
                throw new Exception("Config - There was one or more problems with the config, exiting...");
            }
        }*/
    }
}