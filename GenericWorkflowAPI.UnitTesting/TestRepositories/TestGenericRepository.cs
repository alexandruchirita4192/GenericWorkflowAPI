using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace GenericWorkflowAPI.UnitTesting
{
    [TestClass]
    public class TestGenericRepository : BaseTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var dbContext = GetInMemoryDbContext();
            var configuration = GetConfiguration();
            var logger = new LoggerConfiguration()
                    .Enrich.WithThreadId()
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(configuration)
                    //.WriteTo.Seq(Configuration.GetSection("Seq").GetValue<string>("Url"))
                    .CreateLogger();
            var entityService = new EntityService<Workflow>();
            var repository = new GenericRepository<Workflow, ApplicationDbContext>(dbContext, logger, entityService);
        }
    }
}