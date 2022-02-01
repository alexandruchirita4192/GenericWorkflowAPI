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
    public class TestGenericCodeRepository : BaseTest
    {
        [TestMethod]
        public async Task TestGetAllWithData()
        {
            ServicesExtensions.RegisterEncodingProvider();
            var cancellationToken = new CancellationToken();
            var repository = GetGenericCodeRepository<Workflow>(isInMemoryDbContext: true);

            var user = GetDefaultUser();
            var uniqueId = DateTime.Now.Ticks;
            var entity = new Workflow(uniqueId);

            await repository.AddAsync(entity, user, cancellationToken);
            Print(entity, "After add async:");

            var oneItemList = await repository.GetAllAsync(new List<string>(), cancellationToken);
            Print(oneItemList, "After get all async (with 1 item):");

            Assert.IsNotNull(oneItemList);
            Assert.AreEqual(1, oneItemList.Count);
            Assert.AreEqual(oneItemList[0], entity);
        }

        [TestMethod]
        public async Task TestGetAllWithoutData()
        {
            ServicesExtensions.RegisterEncodingProvider();
            var cancellationToken = new CancellationToken();
            var repository = GetGenericCodeRepository<Workflow>(isInMemoryDbContext: true);

            var emptyList = await repository.GetAllAsync(new List<string>(), cancellationToken);
            Print(emptyList, "After get all async (without data):");

            Assert.IsNotNull(emptyList);
            Assert.AreEqual(0, emptyList.Count);
        }

        [TestMethod]
        public async Task TestGetByInvalidCode()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var repository = GetGenericCodeRepository<Workflow>(isInMemoryDbContext: true);

            var entity = await repository.GetByCodeAsync("InvalidCode", new List<string>(), cancellationToken);
            Print(entity, "After get by code async (without data, so it's an invalid code):");

            Assert.IsNull(entity);
        }

        [TestMethod]
        public async Task TestAddGetUpdateAndDelete()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var repository = GetGenericCodeRepository<Workflow>(isInMemoryDbContext: true);

            var user = GetDefaultUser();
            var uniqueId = DateTime.Now.Ticks;
            var entity = new Workflow(uniqueId);
            var entityCode = entity.Code;
            Assert.IsNotNull(entityCode);
            var entityDescription = entity.Description;
            Assert.IsNotNull(entityDescription);

            await repository.AddAsync(entity, user, cancellationToken);
            Print(entity, "After add async:");

            entity = await repository.GetByCodeAsync(entityCode, new List<string>(), cancellationToken);
            Print(entity, "After get by code async:");

            Assert.IsNotNull(entity);
            Assert.AreEqual(entityCode, entity.Code);
            Assert.AreEqual(entityDescription, entity.Description);
            Assert.IsFalse(entity.IsDeleted);

            var newDescription = entity.Description + "Updated";
            Assert.IsNotNull(newDescription);

            entity.Description = newDescription;
            await repository.UpdateAsync(entity, user, cancellationToken);

            entity = await repository.GetByCodeAsync(entityCode, new List<string>(), cancellationToken);
            Print(entity, "After update description and get by code async:");

            Assert.AreEqual(newDescription, entity.Description);
            Assert.IsFalse(entity.IsDeleted);

            await repository.DeleteAsync(entityCode, user, cancellationToken);
            entity = await repository.GetByCodeAsync(entityCode, new List<string>(), cancellationToken);
            Print(entity, "After delete and get by code async:");

            Assert.IsNull(entity);
        }

        [TestMethod]
        public async Task TestAddRangeGetAndDelete()
        {
            ServicesExtensions.RegisterEncodingProvider();

            var cancellationToken = new CancellationToken();
            var repository = GetGenericCodeRepository<Workflow>(isInMemoryDbContext: true);

            var user = GetDefaultUser();
            var uniqueId = DateTime.Now.Ticks;
            var entity = new Workflow(uniqueId);
            var entityCode = entity.Code;
            Assert.IsNotNull(entityCode);
            var entityDescription = entity.Description;
            Assert.IsNotNull(entityDescription);

            await repository.AddRangeAsync(new List<Workflow> { entity }, user, cancellationToken);
            Print(entity, "After add range async:");

            entity = await repository.GetByCodeAsync(entityCode, new List<string>(), cancellationToken);
            Print(entity, "After get by code async:");

            Assert.IsNotNull(entity);
            Assert.AreEqual(entityCode, entity.Code);
            Assert.AreEqual(entityDescription, entity.Description);
            Assert.IsFalse(entity.IsDeleted);

            var newDescription = entity.Description + "Updated";
            Assert.IsNotNull(newDescription);
            entity.Description = newDescription;
            
            await repository.UpdateAsync(entity, user, cancellationToken);
            entity = await repository.GetByCodeAsync(entityCode, new List<string>(), cancellationToken);
            Print(entity, "After update description and get by code async:");

            Assert.AreEqual(newDescription, entity.Description);
            Assert.IsFalse(entity.IsDeleted);

            await repository.DeleteAsync(new List<string> { entityCode }, user, cancellationToken);
            entity = await repository.GetByCodeAsync(entityCode, new List<string>(), cancellationToken);
            Print(entity, "After delete and get by code async:");

            Assert.IsNull(entity);
        }
    }
}