using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureAuthentication();
            services.ConfigureControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!Utils.IsDebug)
            { app.UseResponseCompression(); }

            app.UseSerilogHttpContextLogger(); // Allows to add additional log properties to Serilog table.
            app.UseSerilogRequestLogging(); // The main package that logs requests.

            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
