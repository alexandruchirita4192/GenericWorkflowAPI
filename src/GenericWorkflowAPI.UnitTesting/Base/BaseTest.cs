using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using GenericWorkflowAPI.AutoMapper;
using GenericWorkflowAPI.Core.AutoMapper;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace GenericWorkflowAPI.UnitTesting
{
    public class BaseTest
    {
        public IConfiguration GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();
            return configuration;
        }

        public ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("db")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }

        public ApplicationDbContext GetSqlServerDbContext(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ApplicationDbContext(options);

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

        public IMapper GetMapper(ILogger logger)
        {
            // Create Entities-DTOs mappings for the Domain assembly
            var domainMappings = new EntityDtoMappingProvider(logger).GetEntityDtoMapping(typeof(Workflow).Assembly);

            var assemblyMappingsList = new List<EntityDtoMapping> { domainMappings };
            var entitiesToDtoProfilesList = new List<EntitiesToDtosProfile>();

            foreach (var assemblyMapping in assemblyMappingsList)
            {
                // Add EntitiesToDtosProfile AutoMapper profile for current assembly to list
                entitiesToDtoProfilesList.Add(new EntitiesToDtosProfile(assemblyMapping?.Mapping));
            }

            var mappingConfig = new MapperConfiguration(mc =>
            {
                foreach (var entitiesToDtosProfile in entitiesToDtoProfilesList)
                    mc.AddProfile(entitiesToDtosProfile);
            });
            var mapper = mappingConfig.CreateMapper();

            return mapper;
        }
    }
}