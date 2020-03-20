using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcClient.ViewModels;
using Newtonsoft.Json;
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
            var viewModel = new ProfileViewModel
            {
                AccessToken = await HttpContext.GetTokenAsync("access_token"),
                IdToken = await HttpContext.GetTokenAsync("id_token"),
                RefreshToken = await HttpContext.GetTokenAsync("refresh_token") // if not explicity configured/asked for, should be null
            };

            if (!string.IsNullOrWhiteSpace(viewModel.AccessToken))
            {
                // UTF8Json can't parse these tokens
                //var token = new JwtSecurityTokenHandler().ReadJwtToken(viewModel.AccessToken);
                //var jsonBytes = JsonSerializer.Serialize(token);
                //viewModel.JsonAccessToken = JsonSerializer.PrettyPrint(jsonBytes);

                // Newtonsoft
                var token = new JwtSecurityTokenHandler().ReadJwtToken(viewModel.AccessToken);
                viewModel.JsonAccessToken = JsonConvert.SerializeObject(token);
            } // Break Point Here To Look At _Token
            // Verify Claim {scope: Mvc.Scope} is available. 

            if (!string.IsNullOrWhiteSpace(viewModel.IdToken))
            {
                // UTF8Json can't parse these tokens
                // var token = new JwtSecurityTokenHandler().ReadJwtToken(viewModel.IdToken);
                // var jsonBytes = JsonSerializer.Serialize(token);
                // viewModel.IdToken = JsonSerializer.PrettyPrint(jsonBytes);

                var token = new JwtSecurityTokenHandler().ReadJwtToken(viewModel.IdToken);
                viewModel.JsonIdToken = JsonConvert.SerializeObject(token);
            } // Break Point Here To Look At _Token
            // Verify Claim {ViewToken: access_token} is now seen. This is inside the Mvc.Scope created in IdentityServer4
            // This is available if the ID Token has added Claims To IdentityToken.

            if (!string.IsNullOrWhiteSpace(viewModel.RefreshToken))
            {
                // UTF8Json can't parse these tokens
                //var token = new JwtSecurityTokenHandler().ReadJwtToken(viewModel.RefreshToken);
                //var jsonBytes = JsonSerializer.Serialize(token);
                //viewModel.RefreshToken = JsonSerializer.PrettyPrint(jsonBytes);

                var token = new JwtSecurityTokenHandler().ReadJwtToken(viewModel.RefreshToken);
                viewModel.JsonRefreshToken = JsonConvert.SerializeObject(token);
            } // Break Point Here To Look At _Token

            return View(viewModel);
        }
    }
}