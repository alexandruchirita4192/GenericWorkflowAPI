// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.Entities.Extensions;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenericWorkflowAPI.Database
{
    public static class SeedDataExtension
    {
        public const string AdministratorUserName = "admin";
        public const string AdministratorEmail = "test@test.test";
        private const string AdministratorPassword = "Pass123$";
        public const string AdministratorRole = "Administrator";
        public const string GenericUserRole = "GenericUser";

        /// <summary>
        /// The way this data is added is not ok, but adding it anyway.
        /// </summary>
        public static async Task EnsureSeedAdminUserData(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            using var serviceProvider = services.BuildServiceProvider(); // TODO: Migrate data and seed without "BuildServiceProvider"
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context =
                configuration.UseSqlServer()
                ? scope.ServiceProvider.GetService<ApplicationDbContext>()
                : (
                    configuration.UseSqlite()
                    ? scope.ServiceProvider.GetService<SqliteApplicationDbContext>()
                    : null
                );

            context?.Database.Migrate();

            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Domain.IdentityRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<Domain.IdentityUser>>();

            IdentityResult? result = null;

            var admin = await userMgr.FindByNameAsync(AdministratorUserName);
            if (admin == null)
            {
                admin = new Domain.IdentityUser(AdministratorUserName)
                {
                    Id = 1,
                    UserName = AdministratorUserName,
                    Email = AdministratorEmail,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                result = await userMgr.CreateAsync(admin, AdministratorPassword);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = await userMgr.AddClaimsAsync(admin, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Test Admin"),
                                new Claim(JwtClaimTypes.GivenName, "Test"),
                                new Claim(JwtClaimTypes.FamilyName, "Admin"),
                                new Claim(JwtClaimTypes.Email, "TestAdmin@Domain.com"),
                                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                                new Claim(JwtClaimTypes.WebSite, "http://example.org"),
                            });
            }

            var adminRole = await roleMgr.FindByNameAsync(AdministratorRole);
            if (adminRole == null)
            {
                result = await roleMgr.CreateAsync(new Domain.IdentityRole(AdministratorRole)
                {
                    CreatedDate = DateTimeOffset.UtcNow,
                    ChangedDate = DateTimeOffset.UtcNow,
                    ChangedByUserId = 1
                });

            }

            var genericUserRole = await roleMgr.FindByNameAsync(AdministratorRole);
            if (genericUserRole == null)
            {
                result = await roleMgr.CreateAsync(new Domain.IdentityRole(GenericUserRole)
                {
                    CreatedDate = DateTimeOffset.UtcNow,
                    ChangedDate = DateTimeOffset.UtcNow,
                    ChangedByUserId = 1
                });
            }


            var roles = await userMgr.GetRolesAsync(admin);
            if (roles == null || roles.Count == 0 || !roles.Contains(AdministratorRole) || !roles.Contains(GenericUserRole))
            {
                result = await userMgr.AddToRolesAsync(admin, new List<string> { AdministratorRole, GenericUserRole });
            }

            if (result != null && !result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }

        public static void RunEmbeddedResourcesInCurrentAssembly(string connectionString, IConfiguration configuration)
        {
            var sqlType = configuration.GetSqlType();
            if (sqlType == null)
                return;

            using var connection = configuration.GetDbConnection();
            if (connection == null)
                return;

            var assembly = Assembly.GetExecutingAssembly();

            var separators = new string[] { "\r\nGO\r\n" };

            foreach (var resourceName in assembly.GetManifestResourceNames().Where(m => m.EndsWith($".{sqlType.ToString()}.sql")))
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                    continue;

                using var reader = new StreamReader(stream);
                if (reader == null)
                    continue;

                var script = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(script)) // Skip empty scripts
                    continue;

                connection.Open();

                foreach (var currentBatch in script.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    using var command = connection.CreateCommand();
                    if (command == null)
                        continue;

                    command.CommandType = CommandType.Text;
                    command.CommandText = currentBatch;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}