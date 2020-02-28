using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IS401_IdentityServer
{
    // For demo purposes, how we quickly define API and Client Resources.
    // This could/should be replaced by loading resources from elsewhere (etc. DB)
    // If your attended resources don't change much though (i.e. one client)
    // and (i.e one API) there is no harm in leaving that hardcoded. It's really
    // up to your setup/needs. It doesn't hurt to always have this loaded
    // dynamically though even if doesn't ever really change.
    public static class InMemoryConfig
    {
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("TestApi"), // API Name
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client // Client that will use the above API.
                {
                    ClientId = "TestApiClient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("TestApiClientSecret".ToSha256()) // extensions provided by IdentityServer
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string>{ "TestApi" }
                },
            };

    }
}
