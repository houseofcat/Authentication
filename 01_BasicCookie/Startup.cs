using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _01_Basics
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(
                    options =>
                    {
                        options.RequireAuthenticatedSignIn = false; // AspNetCore 3.x and simple cookie auth (without a login) needs to be false.
                        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    config =>
                    {
                        config.Cookie.Name = "Basic.Cookie";
                        config.LoginPath = "/Home/Authenticate";
                    });

#if DEBUG
            services
                .AddControllersWithViews()
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

            app.UseCookiePolicy(
                new CookiePolicyOptions
                {
                    MinimumSameSitePolicy = SameSiteMode.None,
                });

            app.UseRouting();

            // Determine the User's identification
            app.UseAuthentication();

            // Determine if a User is Allowed
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });
        }
    }
}
