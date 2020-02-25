using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using System;

namespace IS401_TestClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication("Bearer")
                .AddJwtBearer(
                    "Bearer",
                    options =>
                    {
                        options.Authority = "https://localhost:5001/"; // IdentityServer4
                        options.Audience = "TestApiTwo"; // identify ourselves
                    });

            services
                .AddHttpClient(
                    "IdentityServerClient",
                    options =>
                    {
                        options.BaseAddress = new Uri("https://localhost:5001/"); // this base url is using the IdentityServer address.
                        options.Timeout = TimeSpan.FromSeconds(20);
                    })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(Policies.GetRetryPolicy())
                .AddPolicyHandler(Policies.GetCircuitBreakerPolicy());

            services
                .AddHttpClient(
                    "TestApiClient",
                    options =>
                    {
                        options.BaseAddress = new Uri("https://localhost:5011/api/"); // this base url is using the TestApi address.
                        options.Timeout = TimeSpan.FromSeconds(20);
                    })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(Policies.GetRetryPolicy())
                .AddPolicyHandler(Policies.GetCircuitBreakerPolicy());

            // No longer need to worry about HttpClient Singleton/DnsCachingIssues/Refresh.
            // Just use the HttpClientFactory that solved all (nearly all) of these problems.

            services.AddControllers(); // Adding Just Controllers
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
