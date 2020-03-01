using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var hostBuilder = CreateHostBuilder(args).Build();

                using (var scope = hostBuilder.Services.CreateScope())
                {
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();

                    if (ServiceUtils.IsDebug)
                    {
                        Serilog.Debugging.SelfLog.Enable(Console.WriteLine);
                    }

                    Log.Information("Logger created. Starting IdentityServer...");
                }

                await hostBuilder
                    .RunAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
