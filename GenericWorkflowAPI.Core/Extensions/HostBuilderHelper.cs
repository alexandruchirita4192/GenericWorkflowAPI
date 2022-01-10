using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GenericWorkflowAPI.Core.Extensions
{
    public static class HostBuilderHelper<TStartup, TProgram>
        where TStartup : class
        where TProgram : class
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);
                })

                // TODO: Change environment to Production when it's production-ready (tested with unit testing and maybe integration testing too)
                .UseEnvironment(Environments.Development)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>()
                        .UseSetting(WebHostDefaults.ApplicationKey,
                            typeof(TProgram).Assembly.FullName);
                })

                // Use serilog Logging
                .UseSerilog((hostBuilder, serviceProvider, loggerConfiguration) =>
                {
                    loggerConfiguration.Enrich.WithThreadId()
                        .Enrich.WithThreadName()
                        .Enrich.WithEnvironmentName()
                        .Enrich.WithEnvironmentUserName()
                        .Enrich.WithMachineName()
                        .Enrich.FromLogContext();

                    var configuration = serviceProvider.GetService(typeof(IConfiguration)) as IConfiguration;
                    if (configuration != null)
                        loggerConfiguration.ReadFrom.Configuration(configuration);
                    else
                        throw new InvalidOperationException("Null configuration in UseSerilog in HostBuilderHelper");
                });
        }
    }
}