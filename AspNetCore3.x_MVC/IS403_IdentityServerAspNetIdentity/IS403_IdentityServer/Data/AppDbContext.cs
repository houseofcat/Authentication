using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IS403_IdentityServer.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Set Default Schema For AspNetIdentity
            builder.HasDefaultSchema("Identity");
        }
    }
}
