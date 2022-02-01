using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using AutoMapper;
using GenericWorkflowAPI.AutoMapper;
using GenericWorkflowAPI.Core.AutoMapper;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Responses;
using GenericWorkflowAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.UnitTesting
{
    public class BaseTest
    {
        public Domain.IdentityUser GetDefaultUser()
        {
            return new Domain.IdentityUser("admin") { Id = 1 }; // TODO: This shouldn't be hard-coded
        }

        public IConfiguration GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();
            return configuration;
        }

        public ApplicationDbContext GetSqlServerDbContext(IConfiguration? configuration = null, bool? isInMemory = null, long? ticks = null)
        {
            if (ticks == null)
                ticks = DateTime.Now.Ticks;

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Testing requires detailed errors and filled with sensitive data
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();

            if (isInMemory ?? false)
                optionsBuilder.UseInMemoryDatabase($"db{ticks}");
            else
            {
                if (configuration == null)
                    configuration = GetConfiguration();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectionString);
            }

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            return dbContext;
        }

        public Serilog.Core.Logger GetLogger()
        {
            var logger = new LoggerConfiguration()
                    .Enrich.WithThreadId()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                    .CreateLogger();
            return logger;
        }

        public IMapper GetMapper(ILogger? logger)
        {
            if (logger == null)
                logger = GetLogger();

            // Create Entities-DTOs mappings for the Domain assembly
            var domainMappings = new EntityDtoMappingProvider(logger).GetEntityDtoMapping(typeof(Workflow).Assembly);

            var assemblyMappingsList = new List<EntityDtoMapping> { domainMappings };
            var entitiesToDtoProfilesList = new List<EntitiesToDtosProfile>();

            foreach (var assemblyMapping in assemblyMappingsList)
            {
                // Add EntitiesToDtosProfile AutoMapper profile for current assembly to list
                entitiesToDtoProfilesList.Add(new EntitiesToDtosProfile(assemblyMapping.Mapping));
            }

            var mappingConfig = new MapperConfiguration(mc =>
            {
                foreach (var entitiesToDtosProfile in entitiesToDtoProfilesList)
                    mc.AddProfile(entitiesToDtosProfile);
            });
            var mapper = mappingConfig.CreateMapper();

            return mapper;
        }

        public EntityService<TEntity> GetEntityService<TEntity>()
            where TEntity : class, IBaseEntity, ICodeEntity, new()
        {
            return new EntityService<TEntity>();
        }

        public GenericRepository<TEntity, ApplicationDbContext> GetGenericRepository<TEntity>(
            bool? isInMemoryDbContext = null,
            IConfiguration? configuration = null,
            Serilog.Core.Logger? logger = null,
            ApplicationDbContext? dbContext = null,
            EntityService<TEntity>? entityService = null)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
        {
            if (configuration == null)
                configuration = GetConfiguration();
            if (dbContext == null)
                dbContext = GetSqlServerDbContext(configuration, isInMemoryDbContext);
            if (logger == null)
                logger = GetLogger();
            if (entityService == null)
                entityService = GetEntityService<TEntity>();

            return new GenericRepository<TEntity, ApplicationDbContext>(dbContext, logger, entityService);
        }

        public GenericCodeRepository<TEntity, ApplicationDbContext> GetGenericCodeRepository<TEntity>(
            bool? isInMemoryDbContext = null,
            IConfiguration? configuration = null,
            Serilog.Core.Logger? logger = null,
            ApplicationDbContext? dbContext = null,
            EntityService<TEntity>? entityService = null)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
        {
            if (configuration == null)
                configuration = GetConfiguration();
            if (dbContext == null)
                dbContext = GetSqlServerDbContext(configuration, isInMemoryDbContext);
            if (logger == null)
                logger = GetLogger();
            if (entityService == null)
                entityService = GetEntityService<TEntity>();

            return new GenericCodeRepository<TEntity, ApplicationDbContext>(dbContext, logger, entityService);
        }

        public void AssertGenericApiResponse<T>(GenericApiResponse<T> response, HttpStatusCode? httpStatusCode = null, bool isPayloadNotNull = true)
            where T : class
        {
            Assert.IsNotNull(response);
            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Message));
            if (isPayloadNotNull)
                Assert.IsNotNull(response.Payload);
            else
                Assert.IsNull(response.Payload);
            Assert.IsTrue((int)(response.Status ?? HttpStatusCode.OK) < 400); // 4xx-5xx are error statuses

            if (httpStatusCode != null)
                Assert.AreEqual(httpStatusCode, response.Status, $"HttpStatus was not {httpStatusCode}.");

            Print(response.Payload);
        }

        public void Print<T>(T? payload, string? context = null) where T : class
        {
            if (!string.IsNullOrWhiteSpace(context))
                Console.Write(context + " ");

            if (payload == null)
                Console.WriteLine("Null payload");
            else if (payload is string)
                Console.WriteLine(payload);
            else
                Console.WriteLine(JsonConvert.SerializeObject(payload, Formatting.Indented));
        }
    }
}