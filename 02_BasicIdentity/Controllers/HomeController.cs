using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace _02_BasicIdentity.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<IdentityUser> UserManager { get; set; }
        private SignInManager<IdentityUser> SignInManager { get; set; }

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

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

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoginAsync(string userName, string password)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await SignInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegisterAsync(string userName, string email, string password)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userName,
                    Email = email
                };

                var result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var signInResult = await SignInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    } 
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAsync()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}