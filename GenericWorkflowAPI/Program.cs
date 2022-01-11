using System;
using System.IO;
using GenericWorkflowAPI.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.SystemConsole.Themes;

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
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                    .CreateLogger();

            Log.Logger = logger;

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