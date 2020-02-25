using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IS401_IdentityServerBasics
{
    // How IS4 identifies your API.
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
                    ClientId = "TestClient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("TestClientSecret".ToSha256()) // extensions provided by IdentityServer
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string>{ "TestApi" }
                },
            };

    }
}
