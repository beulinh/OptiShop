using System.ComponentModel.DataAnnotations;

namespace OptiShop.Models;

public class Supplier
{
    [Key]
    public int SupplierId { get; set; }

    [Required, MaxLength(200)]
    public string SupplierName { get; set; } = string.Empty;

    [Required, MaxLength(15)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(255)]
    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<ImportReceipt> ImportReceipts { get; set; } = new List<ImportReceipt>();
}
