using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Extensions;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericWorkflowAPI.UnitTesting
{
    [TestClass]
    public class TestGenericRepository : BaseTest
    {
        [TestMethod]
        public async Task TestGetAllWithData()
        {
            ServicesExtensions.RegisterEncodingProvider();
            var cancellationToken = new CancellationToken();
            var repository = GetGenericRepository<Workflow>(isInMemoryDbContext: true);

            var user = GetDefaultUser();
            var uniqueId = DateTime.Now.Ticks;
            var entity = new Workflow(uniqueId);

            await repository.AddAsync(entity, user, cancellationToken);

            var oneItemList = await repository.GetAllAsync(new List<string>(), cancellationToken);

            Assert.IsNotNull(oneItemList);
            Assert.AreEqual(1, oneItemList.Count);
            Assert.AreEqual(oneItemList[0], entity);
        }

        [TestMethod]
        public async Task TestGetAllWithoutData()
        {
            ServicesExtensions.RegisterEncodingProvider();
            var cancellationToken = new CancellationToken();
            var repository = GetGenericRepository<Workflow>(isInMemoryDbContext: true);

            var emptyList = await repository.GetAllAsync(new List<string>(), cancellationToken);

            Assert.IsNotNull(emptyList);
            Assert.AreEqual(0, emptyList.Count);
        }

        [TestMethod]
        public async Task TestGetByInvalidId()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var repository = GetGenericRepository<Workflow>(isInMemoryDbContext: true);

            var entity = await repository.GetByIdAsync(-1, new List<string>(), cancellationToken);

            Assert.IsNull(entity);
        }

        [TestMethod]
        public async Task TestAddGetUpdateLoadedAndDelete()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var repository = GetGenericRepository<Workflow>(isInMemoryDbContext: true);

            var user = GetDefaultUser();
            var uniqueId = DateTime.Now.Ticks;
            var entity = new Workflow(uniqueId);
            var code = entity.Code;
            Assert.IsNotNull(code);
            var description = entity.Description;
            Assert.IsNotNull(description);

            await repository.AddAsync(entity, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.IsNotNull(entity);
            Assert.AreEqual(code, entity.Code);
            Assert.AreEqual(description, entity.Description);
            Assert.IsFalse(entity.IsDeleted);
            var entityId = entity.Id;
            Assert.AreNotEqual(0, entityId);

            var newDescription = entity.Description + "Updated";
            Assert.IsNotNull(newDescription);
            entity.Description = newDescription;

            await repository.UpdateLoadedAsync(entity, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.AreEqual(newDescription, entity.Description);
            Assert.IsFalse(entity.IsDeleted);

            await repository.DeleteAsync(entityId, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.IsNull(entity);
        }

        [TestMethod]
        public async Task TestAddRangeGetUpdateLoadedAndDelete()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var repository = GetGenericRepository<Workflow>(isInMemoryDbContext: true);

            var user = GetDefaultUser();
            var uniqueId = DateTime.Now.Ticks;
            var entity = new Workflow(uniqueId);
            var code = entity.Code;
            Assert.IsNotNull(code);
            var description = entity.Description;
            Assert.IsNotNull(description);

            await repository.AddRangeAsync(new List<Workflow> { entity }, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.IsNotNull(entity);
            Assert.AreEqual(code, entity.Code);
            Assert.AreEqual(description, entity.Description);
            Assert.IsFalse(entity.IsDeleted);
            var entityId = entity.Id;
            Assert.AreNotEqual(0, entityId);

            var newDescription = entity.Description + "Updated";
            Assert.IsNotNull(newDescription);
            entity.Description = newDescription;

            await repository.UpdateLoadedAsync(entity, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.AreEqual(newDescription, entity.Description);
            Assert.IsFalse(entity.IsDeleted);

            await repository.DeleteAsync(entityId, user, cancellationToken);

            entity = await repository.GetByIdAsync(entity.Id, new List<string>(), cancellationToken);
            Assert.IsNull(entity);
        }
    }
}