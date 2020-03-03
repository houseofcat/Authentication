using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.IO.Compression;
using TestMvcClient;
using Utf8Json.AspNetCoreMvcFormatter;
using Utf8Json.Resolvers;

namespace TestMvClient
{
    public static class StartupExtensions
    {
        public static IConfiguration CreateConfiguration(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{Utils.Env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton(config);

            return config;
        }

        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(
                    options =>
                    {
                        options.DefaultScheme = "Identity.MvcClient.Cookie";
                        options.DefaultChallengeScheme = "oidc"; // oidc Scheme Name
                    })
                .AddCookie("Identity.MvcClient.Cookie")
                .AddOpenIdConnect( // extension is located in Microsoft.AspnetCore.Authentication.OpenIdConnect Nuget
                    "oidc", // oidc Scheme Name
                    options => // scheme configuration
                    {
                        options.Authority = "https://localhost:5001/"; // IdentityServer4
                        options.ClientId = "TestMvcClient";
                        options.ClientSecret = "TestMvcClientSecret"; // keep it secret, keep it safe
                        options.SaveTokens = true;
                        options.ResponseType = "code";
                        options.SignedOutCallbackPath = "/Home/Index";
                    }
                );
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true; // Needed until Utf8Json Mvc Formatters are updated.
            });

            if (Utils.IsDebug) // To Simplify Debugging we don't compress responses and we allow for Razor edits to be viewed at runtime.
            {
                services
                    .AddControllersWithViews(
                        options =>
                        {
                            options.OutputFormatters.Clear(); // Uses faster Utf8Json instead of built-ins.
                            options.OutputFormatters.Add(new JsonOutputFormatter(StandardResolver.ExcludeNull));

                            options.InputFormatters.Clear();
                            options.InputFormatters.Add(new JsonInputFormatter());
                        })
                    .AddRazorRuntimeCompilation();
            }
            else
            {
                services.AddResponseCompression(
                    options =>
                    {
                        options.EnableForHttps = true;
                        options.Providers.Add<GzipCompressionProvider>();
                    });

                services.Configure<GzipCompressionProviderOptions>(
                    options =>
                    {
                        options.Level = CompressionLevel.Optimal;
                    });

                services
                    .AddControllersWithViews(
                        options =>
                        {
                            options.OutputFormatters.Clear();  // Uses faster Utf8Json instead of built-ins.
                            options.OutputFormatters.Add(new JsonOutputFormatter(StandardResolver.ExcludeNull));

                            options.InputFormatters.Clear();
                            options.InputFormatters.Add(new JsonInputFormatter());
                        });
            }
        }
    }
}
