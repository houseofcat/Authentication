using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(IdentityUser user, string password);
        Task<SignInResult> SignInAsync(string emailAddress, string password, bool rememberLogin, bool lockoutOnFailure = true);
        Task SignOutAsync();
    }
}