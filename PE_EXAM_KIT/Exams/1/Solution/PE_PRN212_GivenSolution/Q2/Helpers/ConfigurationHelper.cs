using System.IO;
using Microsoft.Extensions.Configuration;

namespace Q2.Helpers
{
    /// <summary>
    /// Utility to load application configurations and connection strings from appsettings.json.
    /// Requires Nuget Package: Microsoft.Extensions.Configuration.Json
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Retrieves a connection string by name from appsettings.json located in the execution directory.
        /// </summary>
        /// <param name="name">Name of the connection string to retrieve.</param>
        /// <param name="fileName">Configuration filename (defaults to appsettings.json).</param>
        /// <returns>Connection string value or null if not found.</returns>
        public static string GetConnectionString(string name = "DefaultConnection", string fileName = "appsettings.json")
        {
            var basePath = Directory.GetCurrentDirectory();
            
            // Check if appsettings.json exists in execution directory
            var filePath = Path.Combine(basePath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Configuration file '{fileName}' was not found at path '{basePath}'. Make sure 'Copy to Output Directory' is set to 'Copy if newer' or 'Copy always'.");
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(fileName, optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();
            return configuration.GetConnectionString(name);
        }
    }
}
