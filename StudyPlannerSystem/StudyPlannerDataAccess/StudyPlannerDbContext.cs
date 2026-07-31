using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerDataAccess;

public class StudyPlannerDbContext : DbContext
{
    public StudyPlannerDbContext()
    {
    }

    public StudyPlannerDbContext(DbContextOptions<StudyPlannerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; } = null!;
    public virtual DbSet<Subject> Subjects { get; set; } = null!;
    public virtual DbSet<StudyTask> StudyTasks { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string connectionString = configuration.GetConnectionString("DBStudyPlanner")
            ?? throw new InvalidOperationException("Connection string 'DBStudyPlanner' not found in appsettings.json.");

        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Student: StudentCode is unique
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(s => s.StudentCode).IsUnique();
        });

        // Subject: SubjectCode is unique
        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasIndex(s => s.SubjectCode).IsUnique();
        });

        // StudyTask: FK relationships
        modelBuilder.Entity<StudyTask>(entity =>
        {
            entity.HasOne(st => st.Student)
                .WithMany(s => s.StudyTasks)
                .HasForeignKey(st => st.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(st => st.Subject)
                .WithMany(s => s.StudyTasks)
                .HasForeignKey(st => st.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(st => st.IsCompleted)
                .HasDefaultValue(false);
        });
    }
}
