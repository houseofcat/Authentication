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

            // Adding our custom classes/services being setup.
            services.ConfigureServices();

            if (Utils.IsDebug)
            { services.AddControllersWithViews().AddRazorRuntimeCompilation(); }
            else
            { services.AddControllersWithViews(); }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogHttpContextLogger();
            app.UseSerilogRequestLogging();

            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }

            if (Utils.IsDebug) { app.InitializeDatabase(); }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
