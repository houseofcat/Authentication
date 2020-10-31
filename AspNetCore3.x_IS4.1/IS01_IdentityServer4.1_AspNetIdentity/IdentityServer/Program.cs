using HouseofCat.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var hostBuilder = CreateHostBuilder(args).Build();

            var assemblyName = typeof(Program).Assembly.GetName().Name;
            hostBuilder.CreateSerilogLogger(assemblyName);
            Log.Information($"Serilog Logger created. Starting {assemblyName}...");

            await hostBuilder
                .RunAsync()
                .ConfigureAwait(false);
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