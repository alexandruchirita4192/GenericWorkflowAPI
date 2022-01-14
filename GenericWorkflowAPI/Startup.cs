using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GenericWorkflowAPI.AutoMapper;
using GenericWorkflowAPI.CommandHandlers;
using GenericWorkflowAPI.CommandHandlers.RequestHandlers;
using GenericWorkflowAPI.Core.AutoMapper;
using GenericWorkflowAPI.Core.Extensions;
using GenericWorkflowAPI.Core.Filters;
using GenericWorkflowAPI.Core.Helpers;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using GenericWorkflowAPI.Extensions;
using GenericWorkflowAPI.Helpers;
using GenericWorkflowAPI.Services;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace GenericWorkflowAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public IWebHostEnvironment CurrentEnvironment { get; private set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment currentEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = currentEnvironment;
        }

        public bool UseAuthentication
        {
            get
            {
                return string.Compare(Configuration?.GetSection("Authentication")["UseAuthentication"], "true", true) == 0;
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            // Configure the HTTP request pipeline.
            var isDevelopment = env.IsDevelopment(); // app.Environment.IsDevelopment() for .Net 6
            if (isDevelopment)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Generic Workflow API");
                    c.DocumentTitle = "Generic Wokflow API";
                });
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();

                // Show Personal Identifiable Information in development; Some exceptions are hidden because of it
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (UseAuthentication)
                app.UseAuthentication();

            if (isDevelopment)
            {
                // Append AntiForgery token to all calls (making Swagger calls work)
                app.Use(next =>
                {
                    return new RequestDelegate((context) =>
                    {
                        // The request token can be sent as a JavaScript-readable cookie, and Angular uses it by default.
                        var token = antiforgery?.GetAndStoreTokens(context)?.RequestToken;
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            context?.Response?.Cookies?.Append("XSRF-TOKEN", token, new CookieOptions() { HttpOnly = false });

                            // Adding the header on the request to be fine in AntiForgery validation in development by default (mostly needed in Swagger requests)
                            context?.Request?.Headers?.Add("RequestVerificationToken", token);
                        }

                        return next(context);
                    });
                });
            }

            app.UseRouting();

            if (UseAuthentication)
                app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Add serilog logging after UseStaticFiles
            app.UseSerilogRequestLogging(opts =>
            {
                opts.EnrichDiagnosticContext = SerilogLogHelper.EnrichFromRequest;
            });

            // Setup OData
            app.UseODataQueryRequest();

            app.UseCors();

            app.UseApiVersioning();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add Serilog logger
            services.AddLogging(x =>
            {
                x.ClearProviders();
                x.AddSerilog(dispose: true);
            });
            services.AddSingleton(Log.Logger);

            if (UseAuthentication)
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                   .AddJwtBearer(options =>
                   {
                       // Base-address of your identityserver
                       options.Authority = Configuration["Authentication:BaseAddress"];

                       // if you are using API resources, you can specify the name here
                       options.Audience = Configuration["Authentication:Audience"];
                       //options.ClaimsIssuer = Configuration["Authentication:Issuer"];

                       // IdentityServer emits a typ header by default, recommended extra check
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidTypes = new[] { "at+jwt" },
                           ValidIssuer = Configuration["Authentication:Issuer"],
                           ValidateIssuer = true,
                           ValidAudience = Configuration["Authentication:Audience"],
                           ValidateAudience = true,
                           RequireExpirationTime = true
                       };
                       if (options.SecurityTokenValidators.Count == 0)
                           throw new InvalidOperationException("No security token validators");

                       options.Validate();
                   });
            }

            // Add Problem-details middleware:
            services.AddProblemDetails(setup =>
            {
                setup.IncludeExceptionDetails = (ctx, env) => CurrentEnvironment.IsDevelopment() || CurrentEnvironment.IsStaging();
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // Add SQL Server services and exception page
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddTransient<ApplicationDbContext>();

            // Add Identity
            services.AddIdentityCore<Domain.IdentityUser>(options =>
            {
                options.Tokens.AuthenticatorIssuer = Configuration["Authentication:Issuer"];
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<Domain.IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Seed admin user data
            Task.Run(async () =>
            {
                await services.EnsureSeedAdminUserData();
            });

            // Run embedded resource SQL files
            services.RunEmbeddedResourcesInCurrentAssembly(connectionString);

            // Generate EdmModel containing DTOs because those are exposed by API (DTOs are derived from IBaseDto)
            var edmModel = EdmModelHelper.GetEdmModel();

            // Use controllers (with views only for antiforgery validation)
            services.AddControllersWithViews()
                // Add OData with versioning route on a generated EdmModel
                .AddOData(opt => opt.AddRouteComponents("v{version}", edmModel));

            // Add OData query filter (allow filtering for IQueryable)
            services.AddODataQueryFilter();

            // Add API Versioning
            services.AddApiVersioning(options =>
            {
                // Will provide the different api version which is available for the client
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.RegisterMiddleware = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.UseApiBehavior = true;
            });

            // Replacing {version} from Swagger with v1
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.SubstitutionFormat = "VVVV";
                });

            // Add MediatR
            services.AddMediatR(typeof(Workflow).Assembly, typeof(GenericCreateCommandHandler<Workflow, WorkflowDto>).Assembly);

            // Add entities to dtos mapper
            services.AddSingleton(typeof(IEntityDtoMappingProvider), typeof(EntityDtoMappingProvider));

            // Create Entities-DTOs mappings for the Domain assembly
            var domainMappings = new EntityDtoMappingProvider(Log.Logger).GetEntityDtoMapping(typeof(Workflow).Assembly);

            var assemblyMappingsList = new List<EntityDtoMapping> { domainMappings };
            var entitiesToDtoProfilesList = new List<EntitiesToDtosProfile>();

            foreach (var assemblyMapping in assemblyMappingsList)
            {
                // Add EntitiesToDtosProfile AutoMapper profile for current assembly to list
                entitiesToDtoProfilesList.Add(new EntitiesToDtosProfile(assemblyMapping?.Mapping));

                // Entity-related services: EntityService<TEntity> : IEntityService<TEntity>
                services.AddEntityService(assemblyMapping?.Mapping, Log.Logger);

                // Database-related services: GenericRepository<TEntity, TDbContext> : IGenericRepository<TEntity>
                services.AddDatabaseGenericRepositories<ApplicationDbContext>(assemblyMapping?.Mapping, Log.Logger);

                // Database-related services: GenericCodeRepository<TEntity, TDbContext> : IGenericCodeRepository<TEntity>
                services.AddDatabaseGenericCodeRepositories<ApplicationDbContext>(assemblyMapping?.Mapping, Log.Logger);
            }

            // Add AutoMapper IMapper to services
            var mappingConfig = new MapperConfiguration(mc =>
            {
                foreach (var entitiesToDtosProfile in entitiesToDtoProfilesList)
                    mc.AddProfile(entitiesToDtosProfile);
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Add http context accessor
            services.AddHttpContextAccessor();

            // Configure Cross-Origin Resource Sharing (Cors) Policy
            services.ConfigureCors();

            // Register encodings
            Core.Extensions.ServicesExtensions.RegisterEncodingProvider();

            // Add MediatR Handlers based on controllers (and log which handlers fail because entities or dtos do not implement what they should)
            var mediatorMappings = MediatorHelper.GetMappings(Log.Logger);
            services.AddMediatorMappingsToServices(mediatorMappings, Log.Logger);

            // Add workflow service
            services.AddScoped<IWorkflowService, WorkflowService>();

            // Add workflow command handler
            services.AddScoped<IRequestHandler<ExecuteWorkflowRequest, GenericApiResponse<string>>, ExecuteWorkflowCommandHandler>();

            // Swagger/OpenAPI ( https://aka.ms/aspnetcore/swashbuckle ):
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Generic Workflow API", Version = "v1.0" });

                // Add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                c.OperationFilter<OpenApiParameterIgnoreFilter>();
                c.OperationFilter<ODataQueryOptionsFilter>();
                c.DocumentFilter<RemoveSchemasFilter>(assemblyMappingsList);
            });

            // Add AntiForgery service mapping the IAntiforgery interface
            services.AddAntiforgery();
        }
    }
}