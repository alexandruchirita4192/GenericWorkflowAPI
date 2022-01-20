using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Extensions;
using Serilog;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericWorkflowAPI.Domain;
using Microsoft.Extensions.Configuration;

namespace GenericWorkflowAPI.UnitTesting
{
    [TestClass]
    public class TestGenericCodeRepository : BaseTest
    {
        [TestMethod]
        public async Task TestGetAllWithoutData()
        {
            ServicesExtensions.RegisterEncodingProvider();
            var cancellationToken = new CancellationToken();
            var dbContext = GetInMemoryDbContext();
            Serilog.Core.Logger logger = GetLogger();

            var entityService = new EntityService<Workflow>();
            var repository = new GenericCodeRepository<Workflow, ApplicationDbContext>(dbContext, logger, entityService);

            var emptyList = await repository.GetAllAsync(new List<string>(), cancellationToken);

            Assert.IsNotNull(emptyList);
            Assert.AreEqual(0, emptyList.Count);
        }

        [TestMethod]
        public async Task TestGetByInvalidCode()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var dbContext = GetInMemoryDbContext();
            var logger = GetLogger();

            var entityService = new EntityService<Workflow>();
            var repository = new GenericCodeRepository<Workflow, ApplicationDbContext>(dbContext, logger, entityService);

            var entity = await repository.GetByCodeAsync("InvalidCode", new List<string>(), cancellationToken);

            Assert.IsNull(entity);
        }

        [TestMethod]
        public async Task TestInsertGetAndDelete()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var dbContext = GetInMemoryDbContext();
            var logger = GetLogger();

            var entityService = new EntityService<Workflow>();
            var repository = new GenericCodeRepository<Workflow, ApplicationDbContext>(dbContext, logger, entityService);

            var user = new IdentityUser("admin") { Id = 1 };
            var code = $"Test{DateTime.Now}";
            var entity = new Workflow()
            {
                Code = code
            };
            await repository.AddAsync(entity, user, cancellationToken);

            entity = await repository.GetByCodeAsync(code, new List<string>(), cancellationToken);
            Assert.IsNotNull(entity);
            Assert.AreEqual(code, entity.Code);
            Assert.IsFalse(entity.IsDeleted);

            await repository.DeleteAsync(code, user, cancellationToken);

            entity = await repository.GetByCodeAsync(code, new List<string>(), cancellationToken);
            Assert.IsNull(entity);
        }
    }
}