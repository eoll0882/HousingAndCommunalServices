using Microsoft.EntityFrameworkCore;

namespace TaskService.Models
{
    public partial class TaskContext : DbContext
    {
        public TaskContext()
        {
        }

        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Task> Task { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.DeadLine).HasColumnType("date");

                entity.Property(e => e.TaskDescription).HasMaxLength(50);

                entity.Property(e => e.TaskShortName).HasMaxLength(50);
            });
        }
    }
}
