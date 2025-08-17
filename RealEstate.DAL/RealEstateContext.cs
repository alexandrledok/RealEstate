using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RealEstate.DAL
{
    public class RealEstateContext : IdentityDbContext<User>
    {
        public DbSet<Property> Properties { get; set; }
        public DbSet<Space> Spaces { get; set; }
        public RealEstateContext(DbContextOptions<RealEstateContext> options) 
            : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureTables(builder);
            ConfigureRelations(builder);
        }

        private static void ConfigureRelations(ModelBuilder builder)
        {
            builder.Entity<Property>()
                .HasIndex(p => new { p.Price, p.Type });

            builder.Entity<Space>()
                .HasIndex(s => new { s.Type, s.Size });

            builder.Entity<Space>()
                .HasOne(s => s.Property)
                .WithMany(p => p.Spaces)
                .HasForeignKey(s => s.PropertyId);
        }

        public static void ConfigureTables(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UsersRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UsersPermissions");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RolePermissions");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

        }
    }
}
