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
            services
                .AddIdentity<IdentityUser, IdentityRole>() // Default User / Role Object
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

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
