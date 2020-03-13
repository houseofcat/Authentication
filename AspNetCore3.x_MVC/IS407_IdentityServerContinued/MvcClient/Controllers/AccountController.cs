using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {

        }

        [Authorize]
        public async Task<IActionResult> ProfileAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token"); // if not explicity configured/asked for, should be null

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                var _accessToken = new JwtSecurityTokenHandler()
                    .ReadJwtToken(accessToken);
            } // Break Point Here To Look At _Token
            // Verify Claim {scope: Mvc.Scope} is available. 

            if (!string.IsNullOrWhiteSpace(idToken))
            {
                var _idToken = new JwtSecurityTokenHandler()
                    .ReadJwtToken(idToken);
            } // Break Point Here To Look At _Token
            // Verify Claim {ViewToken: access_token} is now seen. This is inside the Mvc.Scope created in IdentityServer4
            // This is available if the ID Token has added Claims To IdentityToken.

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var _refreshToken = new JwtSecurityTokenHandler()
                    .ReadJwtToken(refreshToken);
            } // Break Point Here To Look At _Token

            return View();
        }
    }
}