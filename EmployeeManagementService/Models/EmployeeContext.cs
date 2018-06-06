using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EmployeeManagementService.Models
{
    public partial class EmployeeContext : DbContext
    {
        public EmployeeContext()
        {
        }

        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Cash).HasColumnType("money");

                entity.Property(e => e.DateBirth).HasColumnType("date");

                entity.Property(e => e.EmployeeName).HasMaxLength(50);

                entity.Property(e => e.Post).HasMaxLength(50);

                entity.Property(e => e.Salary).HasColumnType("money");
            });
        }
    }
}
