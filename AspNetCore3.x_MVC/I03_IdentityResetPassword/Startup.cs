using _04_IdentityResetPassword.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace _04_IdentityResetPassword
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("locMem");
            });

            services
                .AddIdentity<IdentityUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequiredLength = 4;
                        options.Password.RequireDigit = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.SignIn.RequireConfirmedEmail = true;
                    })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(
                options =>
                {
                    options.Cookie.Name = "Identity.EmailConfirmation";
                    options.LoginPath = "/Home/Login";
                    options.LogoutPath = "/Home/Logout";
                });

            var mailKitOptions = _config.GetSection("MailKitOptions").Get<MailKitOptions>();
            services.AddMailKit(options => options.UseMailKit(mailKitOptions));
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
