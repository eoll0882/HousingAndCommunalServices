using Microsoft.EntityFrameworkCore;

namespace RoleService.Models
{
    public partial class RoleContext : DbContext
    {
        public RoleContext()
        {
        }

        public RoleContext(DbContextOptions<RoleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Role { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName).HasMaxLength(50);
            });
        }
    }
}
