using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAdminController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("remote")]
        public async Task<IActionResult> PerformRemoteAdminActionAsync()
        {
            // Retrieve the named Client configured in memory.
            // The BaseUrl was established in Startup.cs
            var identityClient = _httpClientFactory.CreateClient("IdentityServerClient");
            await Console.Out.WriteLineAsync("Got Discovery Doc!");

            var wellKnownDiscoveryDocument = await identityClient.GetDiscoveryDocumentAsync(); // No Url needed for this client.

            var credentialResponse = await identityClient
                .RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = wellKnownDiscoveryDocument.TokenEndpoint,
                        ClientId = "TestApiClient",
                        ClientSecret = "TestApiClientSecret", // no need to hash.
                        Scope = "TestApi"
                    });

            if (credentialResponse.IsError)
            { return UnprocessableEntity(); }

            await Console.Out.WriteLineAsync("Succesfully got token!");

            var apiClient = _httpClientFactory.CreateClient("TestApiClient");
            apiClient.SetBearerToken(credentialResponse.AccessToken);

            var response = await apiClient.GetAsync("admin/PerformAdminAction");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return Ok(
                new
                {
                    access_token = credentialResponse.AccessToken,
                    message = await response.Content.ReadAsStringAsync()
                });
        }
    }
}