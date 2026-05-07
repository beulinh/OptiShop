using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptiShop.Models;

public class ImportDetail
{
    [Key]
    public int ImportDetailId { get; set; }

    public int ImportId { get; set; }
    public int ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,0)")]
    public decimal UnitPrice { get; set; }

    // Navigation
    [ForeignKey("ImportId")]
    public ImportReceipt ImportReceipt { get; set; } = null!;

    [ForeignKey("ProductId")]
    public Product Product { get; set; } = null!;
}
