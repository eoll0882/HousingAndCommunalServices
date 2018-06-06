using Microsoft.EntityFrameworkCore;

namespace ProjectService.Models
{
    public partial class ProjectContext : DbContext
    {
        public ProjectContext()
        {
        }

        public ProjectContext(DbContextOptions<ProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Project { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Location).HasMaxLength(50);

                entity.Property(e => e.ProjectName).HasMaxLength(50);
            });
        }
    }
}
