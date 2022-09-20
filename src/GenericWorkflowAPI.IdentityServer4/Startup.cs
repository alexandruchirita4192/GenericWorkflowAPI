// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.Entities.Extensions;

namespace GenericWorkflowAPI.IdentityServer4
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var connectionString = Configuration.GetConnectionString();
            var useSqlite = Configuration.UseSqlite();
            var useSqlServer = Configuration.UseSqlServer();

            if (useSqlite)
            {
                services.AddDbContext<SqliteApplicationDbContext>(options =>
                {
                    options.UseSqlite(connectionString);
                });
            }
            else if (useSqlServer)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            }

            services.AddDatabaseDeveloperPageExceptionFilter();

            var identityBuilder = services.AddIdentity<Domain.IdentityUser, Domain.IdentityRole>();
            if (useSqlite)
                identityBuilder = identityBuilder.AddEntityFrameworkStores<SqliteApplicationDbContext>();
            else if (useSqlServer)
                identityBuilder = identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();
            identityBuilder.AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<Domain.IdentityUser>();

            if (Environment.IsDevelopment())
                builder.AddDeveloperSigningCredential();
            else
            {
                // TODO: Fix for production: not recommended for production - you need to store your key material somewhere secure
            }

            services.AddAuthentication();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}