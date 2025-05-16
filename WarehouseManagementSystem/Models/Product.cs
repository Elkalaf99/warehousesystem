using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSystem.Models;

/// <summary>
/// Represents a product in the warehouse inventory system.
/// </summary>
public class Product
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    [Key]
    public int ProductID { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the initial balance of the product.
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BeginningBalance { get; set; }

    /// <summary>
    /// Gets or sets the collection of transactions associated with this product.
    /// </summary>
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    /// <summary>
    /// Gets the current balance of the product based on transactions.
    /// </summary>
    [NotMapped]
    public decimal CurrentBalance => BeginningBalance + Transactions.Sum(t => t.Type == 'I' ? t.Quantity : -t.Quantity);
} 