using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> UserManager { get; }
        private SignInManager<IdentityUser> SignInManager { get; }

        public UserService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
        {
            return await UserManager
                .CreateAsync(user, password);
        }

        public async Task<SignInResult> SignInAsync(string emailAddress, string password, bool rememberLogin, bool lockoutOnFailure = true)
        {
            return await SignInManager
                .PasswordSignInAsync(emailAddress, password, rememberLogin, lockoutOnFailure: lockoutOnFailure)
                .ConfigureAwait(true);
        }

        public async Task SignOutAsync()
        {
            await SignInManager
                .SignOutAsync()
                .ConfigureAwait(true);
        }
    }
}
