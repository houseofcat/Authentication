using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(), // required scopes for OpenID 2.0
                new IdentityResources.Profile(),
                new IdentityResources.Email()
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
                    ClientId = "TestApiClient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("TestApiClientSecret".ToSha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string>{ "TestApi" },
                },
                new Client
                {
                    ClientId = "TestMvcClient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("TestMvcClientSecret".ToSha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code, // Different GrantType

                    RedirectUris = new List<string>{ "https://localhost:5011/signin-oidc" },
                    PostLogoutRedirectUris = new List<string>{ "https://localhost:5011/Home/Index" },

                    AllowedScopes = new List<string>
                    {
                        "TestApi",
                        "TestApiClient",
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                    },

                    RequireConsent = false, // Only temporary till we are ready to handle user consent.
                },
            };

    }
}
