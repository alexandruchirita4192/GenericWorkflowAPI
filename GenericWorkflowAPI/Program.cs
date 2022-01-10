using System;
using System.IO;
using GenericWorkflowAPI.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;

namespace GenericWorkflowAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();

            var logger = new LoggerConfiguration()
                    .Enrich.WithThreadId()
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(configuration)
                    //.WriteTo.Seq(Configuration.GetSection("Seq").GetValue<string>("Url"))
                    .CreateLogger();

            SelfLog.Enable(message =>
            {
                logger?.Information("Serilog SelfLog message: {message}", message);
            });

            try
            {
                logger?.Information("Starting web application...");

                var builder = HostBuilderHelper<Startup, Program>.CreateHostBuilder(args);
                builder.Build().Run();
            }
            catch (Exception ex)
            {
                logger?.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                logger?.Dispose();
            }
        }
    }
}