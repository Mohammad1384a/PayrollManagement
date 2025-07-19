using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task.Core.Model;

namespace Task.Infrastructure.Data;
public class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options) {
    public DbSet<EmployeePayRoll> EmployeePayrolls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // Fluent API configuration
        modelBuilder.Entity<EmployeePayRoll>(entity =>
        {
            entity.ToTable("EmployeePayrolls");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.BasicSalary).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Allowance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Transportation).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalSalary).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Date).HasColumnType("date");
        });
    }
}
