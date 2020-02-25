using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace _05_CookieAuthPolicy.Authorization
{
    public class CookieRequireClaimHandler : AuthorizationHandler<CookieRequireClaim>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CookieRequireClaim requirement)
        {
            if (context.User.Claims.Any(x => x.Type == requirement.ClaimType))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
