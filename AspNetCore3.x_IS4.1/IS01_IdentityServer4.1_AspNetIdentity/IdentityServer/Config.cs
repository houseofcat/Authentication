using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {
        // User Role Mapping
        private static IdentityResource RoleResource = new IdentityResource
        {
            Name = "role",
            DisplayName = "Role",
            Description = "Allow the service access to your user roles.",
            UserClaims = new[] { JwtClaimTypes.Role, ClaimTypes.Role },
            ShowInDiscoveryDocument = true,
            Required = true,
            Emphasize = true
        };

        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                RoleResource,
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            { };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "ClientOne",
                    RedirectUris =           { "https://localhost:5003/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5003/" },
                    FrontChannelLogoutUri =  "https://localhost:5003/signout-oidc",

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("client.secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        ClaimTypes.Role,  // User Role Mapping
                        JwtClaimTypes.Role, // User Role Mapping
                    },
                    RequireConsent = false,
                }
            };
    }
}