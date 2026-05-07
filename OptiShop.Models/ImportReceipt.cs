using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptiShop.Models;

public class ImportReceipt
{
    [Key]
    public int ImportId { get; set; }

    public int SupplierId { get; set; }
    public int UserId { get; set; }

    public DateTime ImportDate { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,0)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    // Navigation
    [ForeignKey("SupplierId")]
    public Supplier Supplier { get; set; } = null!;

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    public ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();
}
