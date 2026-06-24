using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Assignment
{
    public static class DbConnectionTester
    {
        public static async Task<(bool Success, string Message, string ConnectionString)> TestConnectionAsync()
        {
            string connectionString = string.Empty;
            try
            {
                // Retrieve the connection string from appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                connectionString = configuration.GetConnectionString("DBDefault") ?? string.Empty;
                if (string.IsNullOrEmpty(connectionString))
                {
                    return (false, "Connection string 'DBDefault' was not found in appsettings.json.", string.Empty);
                }

                // Attempt to open a connection to the SQL Server
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return (true, "Successfully connected to the database!", connectionString);
            }
            catch (Exception ex)
            {
                return (false, $"Connection failed: {ex.Message}", connectionString);
            }
        }
    }
}
