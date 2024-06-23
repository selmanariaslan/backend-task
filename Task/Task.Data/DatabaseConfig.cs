using Microsoft.Extensions.Configuration;
using Task.Core.Utils;
using Task.Core.Utils.Security;

namespace Task.Data
{
    public static class DatabaseConfig
    {
        public static string GetConnectionString(string database = "CurrentDatabase", bool isEncryptedString = true)
        {
            var root = ConfigUtils.GetConfigurationRoot();

            var sqlConnection = root.GetConnectionString(root.GetSection("Application").GetSection("Database")[database]);

            if (sqlConnection is not null)
            {
                if (isEncryptedString)
                {
                    var crypto = new CryptoUtils();
                    return crypto.DecryptString(sqlConnection);
                }
                return sqlConnection;
            }
            else
            {
                return null;
            }
        }

        public static string? GetServiceString(string serviceStr = "CurrentService", bool isEncryptedString = true)
        {
            var root = ConfigUtils.GetConfigurationRoot();

            var sqlConnection = root.GetConnectionString(root.GetSection("Application").GetSection("Service")[serviceStr]);


            if (sqlConnection is not null)
            {
                if (isEncryptedString)
                {
                    var crypto = new CryptoUtils();
                    return crypto.DecryptString(sqlConnection);
                }
                return sqlConnection;
            }
            else
            {
                return null;
            }
        }
    }
}