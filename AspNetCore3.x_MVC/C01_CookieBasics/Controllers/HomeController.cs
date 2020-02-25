using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _01_Basics.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Verify if a user is allowed to reach this route.
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        public async Task<IActionResult> Authenticate()
        {
            var mainIdentity = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Test"),
                    new Claim(ClaimTypes.Email, "test@email.com"),
                },
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var externalProviderIdentity = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim("ExternalProvider", "ExternalCompany"),
                    new Claim("ExternalEmailAccount", "test@email.com"),
                    new Claim("ExternalId", Guid.NewGuid().ToString()),
                });

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity[] { mainIdentity, externalProviderIdentity });
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

            return RedirectToAction("Index");
        }
    }
}