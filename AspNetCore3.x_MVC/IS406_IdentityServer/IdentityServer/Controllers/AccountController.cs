using AutoMapper;
using IdentityServer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private IMapper Mapper { get; }
        private UserManager<IdentityUser> UserManager { get; }
        private SignInManager<IdentityUser> SignInManager { get; }

        #region Constants
        private const string UserRegisteredTemplate = "Sucessfully registered user {0}.";
        private const string UserRegistrationFailedKey = "Failed";
        private const string UserRegistrationFailedTemplate = "Failed register user {0}.";
        private const string UserRegisteredAdditionalErrorTemplate = "\r\nError {0}: {1}";
        #endregion

        public AccountController(
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            Mapper = mapper;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            // AutoMap the view model into a new IdentityUser
            var user = Mapper.Map<IdentityUser>(model);

            IdentityResult identityResult;
            try
            { identityResult = await UserManager.CreateAsync(user, model.Password); }
            catch (Exception ex)
            {
                var errorMessage = Utils.Write(UserRegistrationFailedTemplate, user.NormalizedUserName);
                ModelState.AddModelError(UserRegistrationFailedKey, errorMessage);

                Log.Error(ex, errorMessage);

                return View(model);
            }

            // If Successful, sign the user in.
            if (identityResult.Succeeded)
            {
                await SignInManager.SignInAsync(user, model.RememberMe);
                return Redirect(model.ReturnUrl ?? "/");
            }
            else // Else - for now - lets add all the Errors to ModelState to display them on the Register.cshtml page.
            {
                for(int i = 0; i < identityResult.Errors.Count(); i++)
                {
                    ModelState.AddModelError(identityResult.Errors.ElementAt(i).Code, identityResult.Errors.ElementAt(i).Description);
                }

                return View(model);
            }
        }
    }
}