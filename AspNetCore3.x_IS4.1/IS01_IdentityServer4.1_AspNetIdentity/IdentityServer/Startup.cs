using IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Utils;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddControllersWithViews();

            // Found in Utils
            services.ConfigureAspNetIdentityWithSqlServer<ApplicationDbContext>(connectionString);

            // Found in Utils
            services.ConfigureIdentityServer(
                typeof(Startup).GetTypeInfo().Assembly.GetName().Name,
                "Identity",
                connectionString,
                null); // Certificate Issuer is used for looking to find a cert to sign credentials. Should set this in Prod.

            services.ConfigureApplicationCookie(
                options =>
                {
                    options.Cookie.Name = "identityserver.cookie";
                    options.LoginPath = "/Identity/Account/Login"; // Set login path (if not default)
                    options.LogoutPath = "/Identity/Account/Logout"; // Set logout path (if not default)
                });

            services.AddAuthentication();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}