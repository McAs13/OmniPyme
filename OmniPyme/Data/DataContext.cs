using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Web.Data.Entities;

namespace OmniPyme.Data
{
    public class DataContext : IdentityDbContext<Users, Role, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // DbSets
        public DbSet<Client> Clients { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        // Acceso a la tabla AspNetUsers
        public DbSet<Users> ApplicationUsers => Users;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIndexes(builder);
            ConfigureRolePermissionRelations(builder);
            ConfigureUserToRoleRelation(builder);

            // Índice único para DNI
            builder.Entity<Client>()
                .HasIndex(c => c.DNI)
                .IsUnique();
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            builder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();
        }

        private void ConfigureRolePermissionRelations(ModelBuilder builder)
        {
            builder.Entity<RolePermission>()
                .HasKey(rp => new { rp.Roleid, rp.permissionid });

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.Roleid);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.permissionid);
        }

        private void ConfigureUserToRoleRelation(ModelBuilder builder)
        {
            // Evita el conflicto de cascadas múltiples
            builder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.Roleid)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
