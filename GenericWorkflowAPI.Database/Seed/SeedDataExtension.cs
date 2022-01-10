// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
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
        public static void EnsureSeedAdminUserData(this IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                    {
                        context.Database.Migrate();

                        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<Domain.Entities.IdentityUser>>();
                        var admin = userMgr.FindByNameAsync("admin").Result;
                        if (admin == null)
                        {
                            admin = new Domain.Entities.IdentityUser
                            {
                                UserName = "admin"
                            };
                            
                            var result = userMgr.CreateAsync(admin, "Pass123$").Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            result = userMgr.AddClaimsAsync(admin, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Test Admin"),
                                new Claim(JwtClaimTypes.GivenName, "Test"),
                                new Claim(JwtClaimTypes.FamilyName, "Admin"),
                                new Claim(JwtClaimTypes.Email, "TestAdmin@Domain.com"),
                                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                                new Claim(JwtClaimTypes.WebSite, "http://example.org"),
                            }).Result;

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
