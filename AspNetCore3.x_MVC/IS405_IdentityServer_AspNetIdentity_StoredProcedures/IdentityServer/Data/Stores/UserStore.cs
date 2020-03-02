using IdentityServer.Data.Utils;
using IdentityServer.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Data.Stores
{
    // This looks incredibly janky. That's because you are placed in a bit of corner with EF design having to be accounted for.
    // This means a lot of extra functions that essentially don't do anything. This looks like a lot like crappy Java
    // Getters/Setters etc.
    public class UserStore :
        //IUserStore<UserIdentity>, // Not actually needed anymore, Interface is included in the other stores!
        IUserEmailStore<UserIdentity>,
        IUserPhoneNumberStore<UserIdentity>,
        IUserTwoFactorStore<UserIdentity>,
        IUserPasswordStore<UserIdentity>
    {
        private string ConnectionString { get; }

        public UserStore(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("Identity");
        }

        public async Task<IdentityResult> CreateAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.UserId = await DataSource
                .InsertAndReturnValueAsync<UserIdentity, long>(user, ConnectionString, StoredProcedures.UserIdentity.Create);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await DataSource
                .ExecuteAsync(new { Id = user.UserId }, ConnectionString, StoredProcedures.UserIdentity.Delete);

            return IdentityResult.Success;
        }

        public async Task<UserIdentity> FindByEmailAsync(string emailAddress, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await DataSource
                .GetAsync<UserIdentity>(ConnectionString, StoredProcedures.UserIdentity.FindByEmail, new { NormalizedEmailAddress = emailAddress });
        }

        public async Task<UserIdentity> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (long.TryParse(userId, out var numberUserId)) // adjustments need to be made since UserId for us is a LONG
            {
                return await DataSource
                    .GetAsync<UserIdentity>(ConnectionString, StoredProcedures.UserIdentity.FindById, new { UserId = numberUserId });
            }
            else
            {
                return null;
            }
        }

        public async Task<UserIdentity> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await DataSource
                .GetAsync<UserIdentity>(ConnectionString, StoredProcedures.UserIdentity.FindByName, new { NormalizedUserName = userName });
        }

        public Task<string> GetNormalizedUserNameAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId.ToString());
        }

        public Task<string> GetUserNameAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(UserIdentity user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(UserIdentity user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await DataSource
                .UpdateAsync(user, ConnectionString, StoredProcedures.UserIdentity.Update);

            return IdentityResult.Success;
        }

        public Task SetEmailAsync(UserIdentity user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(UserIdentity user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedEmailAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(UserIdentity user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberAsync(UserIdentity user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task<string> GetPhoneNumberAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(UserIdentity user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetTwoFactorEnabledAsync(UserIdentity user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetPasswordHashAsync(UserIdentity user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(UserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public void Dispose()
        { }
    }
}
