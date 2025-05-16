using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSystem.Models;

/// <summary>
/// Represents a transaction in the warehouse inventory system.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Gets or sets the unique identifier for the transaction.
    /// </summary>
    [Key]
    public int TransactionID { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the associated product.
    /// </summary>
    [Required]
    public int ProductID { get; set; }

    /// <summary>
    /// Gets or sets the quantity involved in the transaction.
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Gets or sets the type of transaction ('I' for In, 'O' for Out).
    /// </summary>
    [Required]
    [StringLength(1)]
    public char Type { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the transaction.
    /// </summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets optional notes for the transaction.
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the navigation property to the associated product.
    /// </summary>
    [ForeignKey("ProductID")]
    public virtual Product? Product { get; set; }
} 