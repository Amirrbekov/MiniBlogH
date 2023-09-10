using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MiniBlogWeb.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed Roles (User, Admin, SuperAdmin)

        string adminRoleId = "5f824768-1d88-4faa-853a-530da828e8b8";
        string superAdminRoleId = "ea7bfb08-47e1-4466-9167-875b4e0695f8";
        string userRoleId = "0b0399f6-6bf3-4e46-9aaf-9e33620d5e81";

        List<IdentityRole> roles = new()
        {
            new IdentityRole()
            {
                Name = "admin",
                NormalizedName = "admin",
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
            },

            new IdentityRole(){
                Name = "SuperAdmin",
                NormalizedName= "SuperAdmin",
                Id = superAdminRoleId,
                ConcurrencyStamp = superAdminRoleId
            },

            new IdentityRole(){
                Name = "User",
                NormalizedName = "User",
                Id = userRoleId,
                ConcurrencyStamp = userRoleId
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);

        string superAdminId = "faf868e1-ba48-4778-bacd-8586e1a8bd77";

        // Seed SuperAdminUser

        var superAdminUser = new IdentityUser
        {
            UserName = "superadmin@blog.com",
            Email = "superadmin@blog.com",
            NormalizedEmail = "superadmin@blog.com".ToLower(),
            NormalizedUserName = "superadmin@blog.com".ToLower(),
            Id = superAdminId
        };

        superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "Superadmin123");

        builder.Entity<IdentityUser>().HasData(superAdminUser);

        // Add All roles to SuperAdmin User

        List<IdentityUserRole<string>> superAdminRoles = new()
        {
            new IdentityUserRole<string>()
            {
                RoleId = adminRoleId,
                UserId = superAdminId
            },
            new IdentityUserRole<string>()
            {
                RoleId = superAdminRoleId,
                UserId = superAdminId
            },
            new IdentityUserRole<string>()
            {
                RoleId = userRoleId,
                UserId = superAdminId
            }
        };

        builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
    }
}
