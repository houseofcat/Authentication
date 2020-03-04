using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Threading.Tasks;

namespace MvcClient
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                await CreateHostBuilder(args)
                    .Build()
                    .CreateSerilogLogger()
                    .RunAsync();
            }
            catch (Exception ex)
            { Log.Fatal(ex, "Host terminated unexpectedly."); }
            finally
            { Log.CloseAndFlush(); }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();

        public static IHost CreateSerilogLogger(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            if (Utils.IsDebug) { Serilog.Debugging.SelfLog.Enable(Console.WriteLine); }

            Log.Information("Serilog Logger created. Starting IdentityServer...");

            return host;
        }
    }
}
