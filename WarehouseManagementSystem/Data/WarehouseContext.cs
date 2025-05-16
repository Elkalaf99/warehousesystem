using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Data;

/// <summary>
/// Represents the database context for the warehouse management system.
/// </summary>
public class WarehouseContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the WarehouseContext class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public WarehouseContext(DbContextOptions<WarehouseContext> options)
        : base(options)
    {
        // Ensure database is created and migrations are applied
        Database.EnsureCreated();
    }

    /// <summary>
    /// Gets or sets the DbSet of products.
    /// </summary>
    public DbSet<Product> Products { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet of transactions.
    /// </summary>
    public DbSet<Transaction> Transactions { get; set; } = null!;

    /// <summary>
    /// Configures the model relationships and constraints.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductID);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.BeginningBalance).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure Transaction entity
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionID);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Type).HasMaxLength(1);
            entity.Property(e => e.Notes).HasMaxLength(500);
            
            entity.HasOne(t => t.Product)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { ProductID = 1, Name = "Laptop Dell XPS 13", BeginningBalance = 10 },
            new Product { ProductID = 2, Name = "Microsoft Surface Pro", BeginningBalance = 15 }
        );

        // Seed Transactions
        modelBuilder.Entity<Transaction>().HasData(
            new Transaction 
            { 
                TransactionID = 1, 
                ProductID = 1, 
                Quantity = 5, 
                Type = 'I', 
                Date = DateTime.Now, 
                Notes = "Initial stock" 
            },
            new Transaction 
            { 
                TransactionID = 2, 
                ProductID = 2, 
                Quantity = 8, 
                Type = 'I', 
                Date = DateTime.Now, 
                Notes = "Initial stock" 
            },
            new Transaction 
            { 
                TransactionID = 3, 
                ProductID = 1, 
                Quantity = 2, 
                Type = 'O', 
                Date = DateTime.Now, 
                Notes = "First order" 
            }
        );
    }
} 