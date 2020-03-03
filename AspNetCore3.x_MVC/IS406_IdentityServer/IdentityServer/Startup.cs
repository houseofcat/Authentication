using IdentityServer.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var config = services.CreateConfiguration();

            services.ConfigureAspNetIdentity(config.GetConnectionString("Identity"));
            services.ConfigureIdentityServer(config.GetConnectionString("Identity"));
            services.ConfigureControllers();
            services.ConfigureServices(); // Adding our custom classes/services setup.
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!Utils.IsDebug)
            { app.UseResponseCompression(); } // This middleware is first (so its the last one hit on the way out for responses).

            app.UseSerilogHttpContextLogger(); // Allows to add additional log properties to Serilog table.
            app.UseSerilogRequestLogging(); // The main package that logs requests.

            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }

            if (Utils.IsDebug) { app.InitializeDatabase(); }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
