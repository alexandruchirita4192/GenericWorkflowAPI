using System.Data.Common;
using System;
using GenericWorkflowAPI.Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace GenericWorkflowAPI.Domain.Entities.Extensions
{
    public static class IConfigurationExtensions
    {
        public const string SqlTypeConfigurationSection = "SqlType";

        public static SqlType? GetSqlType(this IConfiguration configuration)
        {
            var section = configuration.GetSection(SqlTypeConfigurationSection);
            if (section == null)
                return null;

            var sectionValue = section.Value;
            if (string.IsNullOrWhiteSpace(sectionValue))
                return null;

            return Enum.TryParse<SqlType>(sectionValue, true, out var result)
                ? result
                : null;
        }

        public static bool UseSqlServer(this IConfiguration configuration)
        {
            return configuration.GetSqlType() == SqlType.SqlServer;
        }

        public static bool UseSqlite(this IConfiguration configuration)
        {
            return configuration.GetSqlType() == SqlType.Sqlite;
        }

        public static string GetConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("DefaultConnection");
        }

        public static DbConnection? GetDbConnection(this IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString();

            if (configuration.UseSqlServer())
                return new SqlConnection(connectionString);
            else if (configuration.UseSqlite())
                return new SqliteConnection(connectionString);
            else
                return null;
        }
    }
}