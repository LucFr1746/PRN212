using Microsoft.EntityFrameworkCore;
using MyStore2.Models;

namespace MyStore2.Data
{
    public class MyStoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connect to SQL Server local instance with User ID=sa and Password=123123
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MyStoreDB;User Id=sa;Password=123123;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Category table
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.CategoryId);
                entity.Property(c => c.CategoryName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(c => c.Description)
                      .HasMaxLength(255);
            });

            // Configure Product table
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.ProductName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");

                // One-to-Many relation: Category has many Products
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
