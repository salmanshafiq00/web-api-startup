
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApiStartup.Entity;

namespace WebApiStartup.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var  superadminRoleId = "845c6c86-b451-48f0-8023-5678e43a18b8";
            var adminRoleId = "dd7c386d-cf8d-4f09-9605-d6c99a1f8297";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = superadminRoleId,
                    Name = "superadmin",
                    NormalizedName = "superadmin".ToUpper(),
                    ConcurrencyStamp = superadminRoleId
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "admin",
                    NormalizedName = "admin".ToUpper(),
                    ConcurrencyStamp = adminRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
