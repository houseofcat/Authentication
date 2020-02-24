using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NETCore.MailKit.Core;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace _04_IdentityResetPassword.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet("")]
        [Route("Index")]
        public IActionResult Index() => View();

        [Authorize]
        [HttpGet("Profile")]
        public IActionResult Profile() => View();

        [HttpGet("Login")]
        public IActionResult Login() => View();

        [HttpGet("Register")]
        public IActionResult Register() => View();

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword() => View();

        [HttpGet("VerifyEmailSent")]
        public IActionResult VerifyEmailSent() => View();

        [HttpGet("ForgotPasswordEmailSent")]
        public IActionResult ForgotPasswordEmailSent() => View();

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(string userName, string email, string password)
        {
            if (userName == null || email == null || password == null) { return BadRequest(); } // Bad Input

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null) { return BadRequest(); } // User Already Exists
                
            user = new IdentityUser
            {
                UserName = userName,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                if (_userManager.Options.SignIn.RequireConfirmedEmail)
                {
                    var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    emailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));

                    var emailVerificationUrl = Url
                        .Action(
                            nameof(VerifyEmailAsync),
                            "Home",
                            new { user.Id, emailToken },
                            Request.Scheme,
                            Request.Host.ToString());

                    var emailVerificationHtml = $"<a href='{HtmlEncoder.Default.Encode(emailVerificationUrl)}'>Verify Email Address!</a>";

                    await _emailService.SendAsync("test@email.com", "Email Verification", emailVerificationHtml, true);

                    return RedirectToAction("VerifyEmailSent");
                }
                else
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Profile");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmailAsync(string userId, string code)
        {
            if (userId == null || code == null) { return RedirectToAction("Index"); }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) { return BadRequest(); }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded) { return BadRequest(); }

            return View();
        }

        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) { return BadRequest(); }

            // Generate Password Token
            var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            passwordToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordToken));

            // Generate Password Reset Url
            var passwordResetUrl = Url
                .Action(
                    nameof(ResetPasswordAsync),
                    "Home",
                    new { user.Id, passwordToken },
                    Request.Scheme,
                    Request.Host.ToString());

            // Generate an HTML Link from Verification URL
            var passwordResetHtml = $"<a href='{HtmlEncoder.Default.Encode(passwordResetUrl)}'>Click here to reset your password!</a>";

            // Send email through EmailService (MailKit NetCore)
            await _emailService.SendAsync("test@email.com", "Password Reset Request", passwordResetUrl, true);

            return RedirectToAction("ForgotPasswordEmailSent");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) { return BadRequest(); }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded) { return BadRequest(); }

            return View();
        }
    }
}