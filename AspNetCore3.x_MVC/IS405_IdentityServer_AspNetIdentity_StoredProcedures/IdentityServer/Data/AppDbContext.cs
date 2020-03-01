using IdentityServer.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    // This needs to be modified to support the NEW User Objects!
    public class AppDbContext : IdentityDbContext<UserIdentity, UserRole, long> // Custom User/Role object
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Set Default Schema For AspNetIdentity
            builder.HasDefaultSchema("Identity");

            builder.Entity<UserIdentity>( // our new UserIdentity Class
                b =>
                {
                    b.Ignore(u => u.Id); // Do not use this OLD property.
                    // if you forget to Ignore properties this will also be included
                    // in the sql tables alongside your new property (in this case the
                    // table would have Id and UserId) and thats just confusing.

                    b.ToTable("UserIdentities"); // Specifying a plural Table name because we like that.

                    b.HasKey(u => u.UserId); // Our new UserId property is a Primary Key.

                    // I don't think we actually needed this setup, it should be done automatically...
                    // ...but I have seen this fail to get used without the following bits of
                    // code.
                    b
                    .Property(u => u.ConcurrencyStamp)
                    .IsConcurrencyToken();
                    // A concurrency token for use with the optimistic concurrency checking
                }
            );

            builder.Entity<UserRole>( // our new UserRole Class
                b =>
                {
                    b.ToTable("Roles"); // Specifying a new Table name because I like plurals.

                    b
                    .Property(u => u.ConcurrencyStamp)
                    .IsConcurrencyToken();
                }
            );
        }
    }
}
