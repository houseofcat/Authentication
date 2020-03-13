using Microsoft.AspNetCore.Authentication;
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

                        // Disabling Claims coming in the Id Token (keeping them small)
                        // means we need to add a second round trip to get user data.
                        // This means that this Auth is Option 2.
                        // Option 1.) IdentityServer Includes Claims In Id Token
                        //   One Large Token
                        //   One Round Trip
                        // Option 2.) IdentityServer Does Not Include Claims In Id Token
                        //   One Smaller Token
                        //   Two Round Trips
                        options.GetClaimsFromUserInfoEndpoint = true;

                        // We also have to Map this data from 2nd round trip.
                        options.ClaimActions.MapUniqueJsonKey("Mvc.ViewToken", "ViewToken"); // Adding the Custom Claim
                        // A good reason to customize the name (i.e. not call it ViewToken), ViewToken is that in case 
                        // you external system ever changes, you only ever have to change it here. You can then build
                        // out your Authority/Roles/Claims endpoints with having to modify them should this Claim name ever change.

                        options.Scope.Add("Mvc.Scope"); // This service (which is a IS4 client) is requesting the new scope we added in IdentityServer.
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
