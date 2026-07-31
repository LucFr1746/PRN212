using System.IO;
using Microsoft.Extensions.Configuration;

namespace Q2.Helpers
{
    public static class ConfigurationHelper
    {
        public static string GetConnectionString(string name = "DefaultConnection", string fileName = "appsettings.json")
        {
            var basePath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(basePath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Configuration file '{fileName}' not found at '{basePath}'. Ensure 'Copy to Output Directory' is configured.");
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(fileName, optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();
            return configuration.GetConnectionString(name);
        }
    }
}
