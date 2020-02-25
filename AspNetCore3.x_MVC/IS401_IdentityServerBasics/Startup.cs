using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IS401_IdentityServerBasics
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Includes Authentication/Authorization
            services.AddIdentityServer();

#if DEBUG
            services
                .AddControllersWithViews(
                    options =>
                    {
                        options.SuppressAsyncSuffixInActionNames = true;
                    })
                .AddRazorRuntimeCompilation();
#else
            services
                .AddControllersWithViews();
#endif
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });
        }
    }
}
