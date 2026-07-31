using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Q2.Models;

public partial class Prn21226sprB12Context : DbContext
{
    public Prn21226sprB12Context()
    {
    }

    public Prn21226sprB12Context(DbContextOptions<Prn21226sprB12Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Products> Products { get; set; }

    public virtual DbSet<Suppliers> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            try
            {
                optionsBuilder.UseSqlServer(Q2.Helpers.ConfigurationHelper.GetConnectionString("DefaultConnection"));
            }
            catch (System.Exception)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=PRN212_26SprB1_2;User Id=sa;Password=123123;TrustServerCertificate=True");
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B5C62977C");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Products>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD56AC4104");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");

            entity.HasMany(d => d.Supplier).WithMany(p => p.Product)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductSuppliers",
                    r => r.HasOne<Suppliers>().WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductSuppliers_Suppliers"),
                    l => l.HasOne<Products>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductSuppliers_Products"),
                    j =>
                    {
                        j.HasKey("ProductId", "SupplierId");
                    });
        });

        modelBuilder.Entity<Suppliers>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4175FC017");

            entity.Property(e => e.ContactEmail).HasMaxLength(150);
            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
