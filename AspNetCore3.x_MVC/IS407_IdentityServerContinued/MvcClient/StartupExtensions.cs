using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcClient.Middleware;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Utf8Json.Resolvers;

namespace MvcClient
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
                        options.DefaultChallengeScheme = "oidc";
                    })
                .AddCookie("Identity.MvcClient.Cookie")
                .AddOpenIdConnect(
                    "oidc",
                    options =>
                    {
                        options.Authority = "https://localhost:5001/";
                        options.ClientId = "MvcClient";
                        options.ClientSecret = "MvcClientSecret";
                        options.SaveTokens = true;
                        options.ResponseType = "code";
                        options.SignedOutCallbackPath = "/Home/Index";
                        options.Scope.Add("Mvc.Scope"); // NEW Token given to users.
                    }
                );
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            // Testing replacements for Utf8Json Formatters.
            //services.Configure<KestrelServerOptions>(options =>
            //{
            //    options.AllowSynchronousIO = true;
            //});

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
