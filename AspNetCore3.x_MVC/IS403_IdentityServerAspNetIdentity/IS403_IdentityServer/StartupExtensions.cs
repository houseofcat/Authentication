using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IS403_IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IS403_IdentityServer
{
    // Helps declutter Startup.cs
    public static class StartupExtensions
    {
        // Useful if you want to have custom environments manually loaded.
        public static IConfiguration CreateConfiguration(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{ServiceUtils.Env}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables() // optional - allows you to read the system environment variables inside your app.
                    .Build();

            services.AddSingleton(config);

            return config;
        }

        // Configuring everything we need for AspNetIdentity to work.
        public static void ConfigureAspNetIdentity(this IServiceCollection services, string connectionString)
        {
            services
                .AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services
                .AddIdentity<IdentityUser, IdentityRole>() // Default user / role object.
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 4;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._!@#$^&| ";
                options.User.RequireUniqueEmail = true;
            });
        }

        // Configure everything we need for IdentityServer4 to work and with AspNetIdentity.
        public static void ConfigureIdentityServer(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services
                .AddIdentityServer(
                    options =>
                    {
                        options.Events.RaiseErrorEvents = true;
                        options.Events.RaiseInformationEvents = true;
                        options.Events.RaiseFailureEvents = true;
                        options.Events.RaiseSuccessEvents = true;
                    })
                .AddDeveloperSigningCredential() // used for signing tokens (not to be used in prod)
                .AddConfigurationStore(
                    options =>
                    {
                        options.DefaultSchema = "Identity"; // Sets the Schema for IdentityServer4 tables.
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(
                                connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly));
                    })
                .AddOperationalStore(
                    options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(
                                connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly));

                        options.DefaultSchema = "Identity"; // Sets the Schema for IdentityServer4 tables.
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 30;
                    })
                .AddAspNetIdentity<IdentityUser>();
        }

        // Save EntityFramework Migrations, Update Resources.
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            Log.Logger.Information("Initializing the database...");

            using var serviceScope = app
                .ApplicationServices
                .GetService<IServiceScopeFactory>()
                .CreateScope();

            serviceScope
                .ServiceProvider
                .GetRequiredService<AppDbContext>()
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
                foreach (var client in Resources.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Resources.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Resources.GetApis())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
            }

            context.SaveChanges();
        }

        public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
        {
            Log.Logger.Information("Initializing the database...");

            using var serviceScope = app
                .ApplicationServices
                .GetService<IServiceScopeFactory>()
                .CreateScope();

            await serviceScope
                .ServiceProvider
                .GetRequiredService<AppDbContext>()
                .Database
                .MigrateAsync();

            await serviceScope
                .ServiceProvider
                .GetRequiredService<PersistedGrantDbContext>()
                .Database
                .MigrateAsync();

            var context = serviceScope
                .ServiceProvider
                .GetRequiredService<ConfigurationDbContext>();

            await context.Database.MigrateAsync();

            // TODO: Should Clients/Ids/Apis be in memory?
            if (!context.Clients.Any())
            {
                foreach (var client in Resources.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Resources.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Resources.GetApis())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
