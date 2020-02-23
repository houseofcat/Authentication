using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _03_IdentityEmailConfirm.Data
{
    public class AppDbContext : IdentityDbContext
    {
        //DbContext is normally used here, but IdentityDbContext includes all the extra tables for Identity
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
