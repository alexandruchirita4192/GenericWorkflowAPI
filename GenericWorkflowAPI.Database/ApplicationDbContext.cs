using System.IO;
using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GenericWorkflowAPI.Database
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, long>
    {
        #region Constructors

        public ApplicationDbContext()
        {
            // Used for unit tests and repositories constructor params
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Used for API web application
        }

        #endregion Constructors

        #region Workflow entities

        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowInputCodeType> WorkflowInputCodeTypes { get; set; }
        public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
        public DbSet<WorkflowInstanceHistory> WorkflowInstanceHistory { get; set; }
        public DbSet<WorkflowInstanceHistoryInputCode> WorkflowInstanceHistoryInputCodes { get; set; }
        public DbSet<WorkflowInstanceInputCode> WorkflowInstanceInputCodes { get; set; }
        public DbSet<WorkflowState> WorkflowStates { get; set; }
        public DbSet<WorkflowStateInputCodeType> WorkflowStateInputCodeTypes { get; set; }
        public DbSet<WorkflowTransition> WorkflowTransitions { get; set; }
        public DbSet<WorkflowType> WorkflowTypes { get; set; }

        #endregion Workflow entities

        #region Overrides

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Needed for migrations
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        #endregion Overrides
    }
}