using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Q2.Models;

public partial class Prn21226sprB11Context : DbContext
{
    public Prn21226sprB11Context()
    {
    }

    public Prn21226sprB11Context(DbContextOptions<Prn21226sprB11Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Departments> Departments { get; set; }

    public virtual DbSet<Employees> Employees { get; set; }

    public virtual DbSet<Skills> Skills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Q2.Helpers.ConfigurationHelper.GetConnectionString("DefaultConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departments>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BEDAA0CC45A");

            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Employees>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F116B393772");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Departments");

            entity.HasMany(d => d.Skill).WithMany(p => p.Employee)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeSkills",
                    r => r.HasOne<Skills>().WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeeSkills_Skills"),
                    l => l.HasOne<Employees>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeeSkills_Employees"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "SkillId");
                    });
        });

        modelBuilder.Entity<Skills>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skills__DFA09187CC8F6F7C");

            entity.Property(e => e.SkillName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
