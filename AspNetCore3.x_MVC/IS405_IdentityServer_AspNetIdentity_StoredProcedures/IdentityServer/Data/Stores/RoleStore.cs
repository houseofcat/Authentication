using IdentityServer.Data.Utils;
using IdentityServer.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Data.Stores
{
    // Notice a similar level of jankiness as the UserStore.
    public class RoleStore : IRoleStore<UserRole>
    {
        private string ConnectionString { get; }

        public RoleStore(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("Identity");
        }

        public async Task<IdentityResult> CreateAsync(UserRole userRole, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            userRole.Id = await DataSource
                .InsertAndReturnValueAsync<UserRole, long>(userRole, ConnectionString, StoredProcedures.UserRole.Create);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(UserRole userRole, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await DataSource
                .UpdateAsync(userRole, ConnectionString, StoredProcedures.UserRole.Update);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(UserRole userRole, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await DataSource
                .ExecuteAsync(new { userRole.Id }, ConnectionString, StoredProcedures.UserRole.Delete);

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(UserRole userRole, CancellationToken cancellationToken)
        {
            return Task.FromResult(userRole.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(UserRole userRole, CancellationToken cancellationToken)
        {
            return Task.FromResult(userRole.Name);
        }

        public Task SetRoleNameAsync(UserRole userRole, string roleName, CancellationToken cancellationToken)
        {
            userRole.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(UserRole userRole, CancellationToken cancellationToken)
        {
            return Task.FromResult(userRole.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(UserRole userRole, string normalizedName, CancellationToken cancellationToken)
        {
            userRole.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public async Task<UserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (long.TryParse(roleId, out var numberRoleId)) // adjustments need to be made since Id for us is a LONG
            {
                return await DataSource
                    .GetAsync<UserRole>(ConnectionString, StoredProcedures.UserRole.FindById, new { Id = numberRoleId });
            }
            else
            {
                return null;
            }
        }

        public async Task<UserRole> FindByNameAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await DataSource
                .GetAsync<UserRole>(ConnectionString, StoredProcedures.UserRole.FindByName, new { NormalizedRoleName = roleName });
        }

        public void Dispose()
        { }
    }
}
