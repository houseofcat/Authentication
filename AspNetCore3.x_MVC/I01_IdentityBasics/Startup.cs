using _02_BasicIdentity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _02_BasicIdentity
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("locMem"); // EF.Core In-Memory 
            });

            // Adding AspNetCore Identity
            services      // Default User / Role Object - could have been custom users and roles demonstrated later.
                .AddIdentity<IdentityUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequiredLength = 4;
                        options.Password.RequireDigit = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                    }) 
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(
                options =>
                {
                    options.Cookie.Name = "Basic.Identity";
                    options.LoginPath = "/Home/Login";
                    options.LogoutPath = "/Home/Logout";
                });

#if DEBUG
            services
                .AddControllersWithViews(
                    options =>
                    {
                        // Defaults to true (turn to false if you are used to using Async in your Route + method names.)
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

            app.UseRouting(); // where are you going?

            app.UseAuthentication(); // who dis?
            app.UseAuthorization(); // you cool?

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapDefaultControllerRoute(); // create addresses
                });
        }
    }
}
