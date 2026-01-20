using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security;
using UserManagement.RestAPI.Models.Domain;

namespace UserManagement.RestAPI.DBContext
{
    public class UserManagementDBContext : DbContext
    {
        public UserManagementDBContext(DbContextOptions<UserManagementDBContext> options)
            : base(options)
        {

        }

        protected UserManagementDBContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite keys
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserID, ur.RoleID });

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleID, rp.PermissionID });
        }

    }
}
