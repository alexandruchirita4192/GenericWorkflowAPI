using System.IO;
using GenericWorkflowAPI.Domain.Entities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GenericWorkflowAPI.Database
{
    public class SqliteApplicationDbContext : ApplicationDbContext
    {
        #region Constructors

        public SqliteApplicationDbContext()
            : base()
        {
            // Used for unit tests and repositories constructor params
        }

        public SqliteApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Used for API web application
        }

        #endregion

        #region Overrides

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Needed for migrations
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

                var connectionString = configuration.GetConnectionString();
                optionsBuilder.UseSqlite(connectionString);
            }
        }

        #endregion Overrides
    }
}