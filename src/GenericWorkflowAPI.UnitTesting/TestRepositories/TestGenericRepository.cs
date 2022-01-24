using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericWorkflowAPI.UnitTesting
{
    [TestClass]
    public class TestGenericRepository : BaseTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var dbContext = GetSqlServerDbContext(isInMemory: true);
            var logger = GetLogger();
            var entityService = new EntityService<Workflow>();
            var repository = new GenericRepository<Workflow, ApplicationDbContext>(dbContext, logger, entityService);
        }
    }
}