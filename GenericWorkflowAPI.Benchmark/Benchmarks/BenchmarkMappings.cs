using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using GenericWorkflowAPI.AutoMapper;
using GenericWorkflowAPI.Core.AutoMapper;
using GenericWorkflowAPI.Core.Extensions;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace GenericWorkflowAPI.Benchmark
{
    [MemoryDiagnoser]
    [NativeMemoryProfiler]
    public class BenchmarkMappings
    {
        #region Fields

        private readonly Logger _logger;
        private readonly EntityService<Workflow> _entityService;
        private readonly GenericCodeRepository<Workflow, ApplicationDbContext> _repository;
        private readonly IMapper _mapper;
        private readonly List<Workflow> _entities;
        private readonly List<WorkflowDto> _dtos;

        #endregion Fields

        #region Constructors

        public BenchmarkMappings()
        {
            ServicesExtensions.RegisterEncodingProvider();
            var configuration = GetConfiguration();
            var dbContext = GetSqlServerDbContext(configuration);
            _logger = GetLogger();

            _entityService = new EntityService<Workflow>();
            _repository = new GenericCodeRepository<Workflow, ApplicationDbContext>(dbContext, _logger, _entityService);
            _mapper = GetMapper(_logger);

            _entities = _repository.DbSet.Include(w => w.Type).AsNoTracking().ToList();
            _dtos = _mapper.Map<List<WorkflowDto>>(_entities);

            _logger.Information(JsonConvert.SerializeObject(_entities));
            _logger.Information(JsonConvert.SerializeObject(_dtos));
        }

        #endregion Constructors

        #region Internal stuff

        public Serilog.Core.Logger GetLogger()
        {
            var logger = new LoggerConfiguration()
                    .Enrich.WithThreadId()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(LogEventLevel.Debug)
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

        public IConfiguration GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();
            return configuration;
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

        #endregion Internal stuff

        #region Benchmark methods

        [Benchmark]
        public void TestMappingWithIMapper()
        {
            var dtos = _mapper.Map<List<WorkflowDto>>(_entities);
        }

        #endregion Benchmark methods
    }
}