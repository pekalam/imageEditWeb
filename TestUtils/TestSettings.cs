using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace TestUtils
{
    public class TestSettings
    {
        private static readonly Lazy<IConfiguration> Config = new Lazy<IConfiguration>(() =>
        {
            var path = "test.settings.";

            Console.WriteLine(Environment.GetEnvironmentVariable("USE_SETTINGS") != null ? "Using test settings" : "using local test settings");

            if (Environment.GetEnvironmentVariable("USE_SETTINGS") == null)
            {
                path += "local.json";
            }
            else
            {
                path += "json";
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            var config = new ConfigurationBuilder()
                .AddJsonFile(path)
                .Build();

            return config;
        });

        public static IConfiguration Configuration => Config.Value;

        public static string GetValueOrDefault(string key, string defaultValue) => Config.Value[key] ?? defaultValue;
    }
}