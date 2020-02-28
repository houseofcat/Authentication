using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IS401_IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Includes Authentication/Authorization
            services
                .AddIdentityServer()
                .AddInMemoryApiResources(InMemoryConfig.GetApis()) // For basic demonstration, we are going to persist APIs in memory.
                .AddInMemoryClients(InMemoryConfig.GetClients()) // Same here we but for Clients in memory. Ideally these are really persisted / comes from a database/store.
                .AddDeveloperSigningCredential(); // will add a local key to sign tokens - ONLY DEV

            // Without anything else
            // This url will currently be working.
            //https://localhost:5001/.well-known/openid-configuration

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
