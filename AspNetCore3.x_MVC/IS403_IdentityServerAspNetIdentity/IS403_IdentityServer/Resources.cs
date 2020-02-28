using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IS403_IdentityServer
{
    public static class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(), // required scopes for OpenID 2.0
                new IdentityResources.Profile(), // normally optional (required for microsoft openId nuget extensions added to MVC client)
                new IdentityResources.Email() // optional
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("TestApi"),
                new ApiResource("TestApiClient")
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "TestApiClient", // can also be an ApiResource
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("TestApiClientSecret".ToSha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string>{ "TestApi" }
                },
                new Client // New Client
                {
                    ClientId = "TestMvcClient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("TestMvcClientSecret".ToSha256()) // New Secret
                    },
                    AllowedGrantTypes = GrantTypes.Code, // Different GrantType
                    RedirectUris = new List<string> { "https://localhost:5031/signin-oidc" },
                    AllowedScopes = new List<string>
                    {
                        "TestApi",
                        "TestApiClient",
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId, // openid matches ^ OpenId IdentityResource
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                    } 
                },
            };

    }
}
