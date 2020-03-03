using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TestMvClient;
using TestMvClient.Middleware;

namespace TestMvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureAuthentication();

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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
