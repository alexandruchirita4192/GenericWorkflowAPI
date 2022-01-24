using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Extensions;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericWorkflowAPI.UnitTesting
{
    [TestClass]
    public class TestGenericRepository : BaseTest
    {
        [TestMethod]
        public async Task TestGetAllWithoutData()
        {
            ServicesExtensions.RegisterEncodingProvider();
            var cancellationToken = new CancellationToken();
            var repository = GetGenericCodeRepository<Workflow>(isInMemoryDbContext: true);

            var emptyList = await repository.GetAllAsync(new List<string>(), cancellationToken);

            Assert.IsNotNull(emptyList);
            Assert.AreEqual(0, emptyList.Count);
        }

        [TestMethod]
        public async Task TestGetByInvalidId()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var repository = GetGenericCodeRepository<Workflow>(isInMemoryDbContext: true);

            var entity = await repository.GetByIdAsync(-1, new List<string>(), cancellationToken);

            Assert.IsNull(entity);
        }

        [TestMethod]
        public async Task TestAddGetUpdateLoadedAndDelete()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var repository = GetGenericCodeRepository<Workflow>(isInMemoryDbContext: true);

            var user = new IdentityUser("admin") { Id = 1 };
            var code = $"Test{DateTime.Now}";
            var entity = new Workflow()
            {
                Code = code,
                Description = string.Empty
            };

            await repository.AddRangeAsync(new List<Workflow> { entity }, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.IsNotNull(entity);
            Assert.AreEqual(code, entity.Code);
            Assert.AreEqual(string.Empty, entity.Description);
            Assert.IsFalse(entity.IsDeleted);

            var description = $"Description{DateTime.Now}";

            entity.Description = description;
            await repository.UpdateLoadedAsync(entity, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.AreEqual(description, entity.Description);
            Assert.IsFalse(entity.IsDeleted);

            await repository.DeleteAsync(code, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.IsNull(entity);
        }

        [TestMethod]
        public async Task TestAddRangeGetUpdateLoadedAndDelete()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var repository = GetGenericCodeRepository<Workflow>(isInMemoryDbContext: true);

            var user = new IdentityUser("admin") { Id = 1 };
            var code = $"Test{DateTime.Now}";
            var entity = new Workflow()
            {
                Code = code,
                Description = string.Empty
            };

            await repository.AddRangeAsync(new List<Workflow> { entity }, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.IsNotNull(entity);
            Assert.AreEqual(code, entity.Code);
            Assert.AreEqual(string.Empty, entity.Description);
            Assert.IsFalse(entity.IsDeleted);

            var description = $"Description{DateTime.Now}";

            entity.Description = description;
            await repository.UpdateLoadedAsync(entity, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.AreEqual(description, entity.Description);
            Assert.IsFalse(entity.IsDeleted);

            await repository.DeleteAsync(code, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.IsNull(entity);
        }
    }
}