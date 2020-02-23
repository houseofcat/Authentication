using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Threading.Tasks;

namespace _03_IdentityEmailConfirm.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private UserManager<IdentityUser> UserManager { get; set; }
        private SignInManager<IdentityUser> SignInManager { get; set; }
        private IEmailService EmailService { get; set; }

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            EmailService = emailService;
        }

        [HttpGet("")]
        [Route("Index")]
        public IActionResult Index() => View(); // Simplified Get Views

        [Authorize]
        [HttpGet("Profile")]
        public IActionResult Profile() => View();

        [HttpGet("Login")]
        public IActionResult Login() => View();

        [HttpGet("EmailSent")]
        public IActionResult EmailSent() => View();

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(string userName, string password)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await SignInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
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
                    // Server options requires email to be confirmed?
                    if (UserManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        // Generate Email Token
                        var emailToken = await UserManager.GenerateEmailConfirmationTokenAsync(user);

                        // Generate Email Verification Url
                        var emailVerification = Url
                            .Action(
                                nameof(VerifyEmailAsync),
                                "Home",
                                new { user.Id, emailToken },
                                Request.Scheme,
                                Request.Host.ToString());

                        // Generate an HTML Link from Verification URL
                        var emailVerificationLink = $"<a href=\"{emailVerification}\">Verify Email Address!</a>";

                        // Send email through EmailService (MailKit NetCore)
                        await EmailService.SendAsync("test@email.com", "Email Verification", emailVerificationLink, true);

                        return RedirectToAction("EmailSent");
                    }
                    else // Sign in
                    {
                        var signInResult = await SignInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                        if (signInResult.Succeeded)
                        {
                            return RedirectToAction("Profile");
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmailAsync(string userId, string code)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null) { return BadRequest(); }

            var result = await UserManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded) { return BadRequest(); }

            return View();
        }
    }
}