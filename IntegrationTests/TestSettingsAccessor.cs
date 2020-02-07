using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace IntegrationTests
{
    public class TestSettingsAccessor
    {
        private static Lazy<Dictionary<string, string>> _dict = new Lazy<Dictionary<string, string>>(() =>
        {
            var path = "test.settings.json";

            Console.WriteLine(Environment.GetEnvironmentVariable("USE_SETTINGS") != null ? "Using test settings" : "");

            var settings = File.Exists(path) && Environment.GetEnvironmentVariable("USE_SETTINGS") != null
                ? JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path))
                : new Dictionary<string, string>();
            if (settings.Count > 0)
            {
                Console.WriteLine($"Following test settings were read from {path}");
            }
            foreach (var (k, v) in settings)
            {
                Console.WriteLine($"{k}: {v}");
            }

            return settings;
        });

        public static Dictionary<string, string> Settings => _dict.Value;

        static TestSettingsAccessor()
        {

        }
    }
}