using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using TestMvcClient;

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
    }
}
