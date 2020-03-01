using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer.Data;
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
using IdentityServer.Identity;

namespace IdentityServer
{
    public static class StartupExtensions
    {
        public static IConfiguration CreateConfiguration(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{ServiceUtils.Env}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            services.AddSingleton(config);

            return config;
        }

        public static void ConfigureAspNetIdentity(this IServiceCollection services, string connectionString)
        {
            services
                .AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services
                .AddIdentity<UserIdentity, UserRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 4;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._!@#$^&| ";
                options.User.RequireUniqueEmail = true;
            });
        }

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
                        options.DefaultSchema = "Identity";
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

                        options.DefaultSchema = "Identity";
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 30;
                    })
                .AddAspNetIdentity<UserIdentity>();
        }

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
