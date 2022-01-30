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
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericWorkflowAPI.Database
{
    public static class SeedDataExtension
    {
        private const string AdministratorUserName = "admin";
        private const string AdministratorPassword = "Pass123$";
        private const string AdministratorRole = "Administrator";
        private const string GenericUserRole = "GenericUser";

        /// <summary>
        /// The way this data is added is not ok, but adding it anyway.
        /// </summary>
        public static async Task EnsureSeedAdminUserData(this IServiceCollection services)
        {
            using var serviceProvider = services.BuildServiceProvider(); // TODO: Migrate data and seed without "BuildServiceProvider"
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            
            context?.Database.Migrate();

            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Domain.IdentityRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<Domain.IdentityUser>>();

            IdentityResult? result = null;

            var admin = await userMgr.FindByNameAsync(AdministratorUserName);
            if (admin == null)
            {
                admin = new Domain.IdentityUser
                {
                    UserName = AdministratorUserName,
                    Email = "test@test.test",
                    EmailConfirmed = true,
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

        public static void RunEmbeddedResourcesInCurrentAssembly(string connectionString)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var separators = new string[] { "\r\nGO\r\n" };

            foreach (var resourceName in assembly.GetManifestResourceNames().Where(m => m.EndsWith(".sql")))
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

                using var connection = new SqlConnection(connectionString);
                if (connection == null)
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