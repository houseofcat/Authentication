using Microsoft.AspNetCore.Authorization;

namespace _05_CookieAuthPolicy.Authorization
{
    public static class PolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(
            this AuthorizationPolicyBuilder b,
            string customClaimType)
        {
            return b.AddRequirements(new CookieRequireClaim(customClaimType));
        }
    }
}
