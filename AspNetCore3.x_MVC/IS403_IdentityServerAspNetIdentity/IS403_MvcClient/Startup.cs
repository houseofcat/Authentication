using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IS403_MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(
                    options =>
                    {
                        options.DefaultScheme = "Identity.MvcClient";
                        options.DefaultChallengeScheme = "oidc"; // oidc Scheme Name
                    })
                .AddCookie("Identity.MvcClient")
                .AddOpenIdConnect( // extension is located in Microsoft.AspnetCore.Authentication.OpenIdConnect Nuget
                    "oidc", // oidc Scheme Name
                    options => // scheme configuration
                    {
                        options.Authority = "https://localhost:5001"; // IdentityServer4
                        options.ClientId = "TestMvcClient";
                        options.ClientSecret = "TestMvcClientSecret"; // keep it secret, keep it safe
                        options.SaveTokens = true;
                        options.ResponseType = "code";
                    }
                );
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });
        }
    }
}
