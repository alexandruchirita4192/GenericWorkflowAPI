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
using GenericWorkflowAPI.Database;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericWorkflowAPI.Database
{
    public static class SeedDataExtension
    {
        private const string AdministratorRole = "Administrator";
        private const string GenericUserRole = "GenericUser";

        /// <summary>
        /// The way this data is added is not ok, but adding it anyway.
        /// </summary>
        public static async Task EnsureSeedAdminUserData(this IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                    {
                        context.Database.Migrate();

                        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Domain.Entities.IdentityRole>>();
                        

                        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<Domain.Entities.IdentityUser>>();
                        var admin = await userMgr.FindByNameAsync("admin");
                        if (admin == null)
                        {
                            admin = new Domain.Entities.IdentityUser
                            {
                                UserName = "admin",
                                Email = "test@test.test",
                                EmailConfirmed = true,
                            };
                            
                            var result = await userMgr.CreateAsync(admin, "Pass123$");
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

                            result = await roleMgr.CreateAsync(new Domain.Entities.IdentityRole(AdministratorRole)
                            {
                                CreatedDate = DateTimeOffset.UtcNow,
                                ChangedDate = DateTimeOffset.UtcNow,
                                ChangedByUserId = 1
                            });

                            result = await roleMgr.CreateAsync(new Domain.Entities.IdentityRole(GenericUserRole)
                            {
                                CreatedDate = DateTimeOffset.UtcNow,
                                ChangedDate = DateTimeOffset.UtcNow,
                                ChangedByUserId = 1
                            });

                            result = await userMgr.AddToRolesAsync(admin, new List<string> { AdministratorRole, GenericUserRole });

                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                        }
                    }
                }
            }
        }

        public static void RunEmbeddedResourcesInCurrentAssembly(this IServiceCollection services, string connectionString)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var separators = new string[] { "\r\nGO\r\n" };

            foreach (var resourceName in assembly.GetManifestResourceNames().Where(m=>m.EndsWith(".sql")))
            {
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream))
                {
                    var script = reader.ReadToEnd();

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        foreach (var currentBatch in script.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                            using (var command = connection.CreateCommand())
                            {
                                command.CommandType = CommandType.Text;
                                command.CommandText = currentBatch;
                                command.ExecuteNonQuery();
                            }
                        connection.Close();
                    }
                }
            }
        }
    }
}
