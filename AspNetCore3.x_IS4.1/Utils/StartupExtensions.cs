using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;

namespace Utils
{
    public static class StartupExtensions
    {
        public static IConfiguration CreateConfiguration(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{Service.Env}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables() // optional - allows you to read the system environment variables inside your app.
                    .Build();

            services.AddSingleton(config);

            return config;
        }

        public static void ConfigureAspNetIdentityWithSqlServer<T>(this IServiceCollection services, string connectionString) where T : IdentityDbContext
        {
            services
                .AddDbContext<T>(options => options.UseSqlServer(connectionString));

            services
                .AddIdentity<IdentityUser, IdentityRole>() // Default user / role object.
                .AddEntityFrameworkStores<T>()
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

        public static void ConfigureIdentityServer(
            this IServiceCollection services,
            string assemblyName,
            string defaultSchema,
            string connectionString,
            string certIssuer)
        {
            var identityServerBuilder = services
                .AddIdentityServer(
                    options =>
                    {
                        options.Events.RaiseErrorEvents = true;
                        options.Events.RaiseInformationEvents = true;
                        options.Events.RaiseFailureEvents = true;
                        options.Events.RaiseSuccessEvents = true;
                    })
                .AddConfigurationStore(
                    options =>
                    {
                        options.DefaultSchema = defaultSchema;
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(
                                connectionString,
                                sql => sql.MigrationsAssembly(assemblyName));
                    })
                .AddOperationalStore(
                    options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(
                                connectionString,
                                sql => sql.MigrationsAssembly(assemblyName));

                        options.DefaultSchema = defaultSchema;
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 30;
                    })
                .AddAspNetIdentity<IdentityUser>();


            if (Service.IsDebug)
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(certIssuer))
                {
                    Log.Error("CertIssuer was not found in the appsettings.json");
                    //throw new InvalidOperationException(errorMessage);
                }

                var certificate = X509.GetCertificateByIssuer(certIssuer);
                if (certificate == null)
                {
                    var errorMessage = $"X509 Certificate (for issuer {certIssuer}) was not found";
                    Log.Error(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                identityServerBuilder.AddSigningCredential(certificate);
            }
        }
    }
}
