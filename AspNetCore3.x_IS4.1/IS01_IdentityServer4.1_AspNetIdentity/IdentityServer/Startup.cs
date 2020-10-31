using IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Linq;
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
            if (Environment.IsDevelopment() || Service.IsDebug)
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                InitializeDatabase(app);
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

        private void InitializeDatabase(IApplicationBuilder app)
        {
            Log.Logger.Information("Initializing the database...");

            using var serviceScope = app
                .ApplicationServices
                .GetService<IServiceScopeFactory>()
                .CreateScope();

            serviceScope
                .ServiceProvider
                .GetRequiredService<ApplicationDbContext>()
                .Database
                .Migrate();

            serviceScope
                .ServiceProvider
                .GetRequiredService<PersistedGrantDbContext>()
                .Database
                .Migrate();

            var context = serviceScope
                .ServiceProvider
                .GetRequiredService<ConfigurationDbContext>();

            context.Database.Migrate();

            // TODO: Should Clients/Ids/Apis be in memory?
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.Ids)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.Apis)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
            }

            context.SaveChanges();
        }
    }
}