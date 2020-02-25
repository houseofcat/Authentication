using Microsoft.AspNetCore.Authorization;

namespace _05_CookieAuthPolicy.Authorization
{
    public class CookieRequireClaim : IAuthorizationRequirement
    {
        public CookieRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }
}
