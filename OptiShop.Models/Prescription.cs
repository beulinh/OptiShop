using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptiShop.Models;

public class Prescription
{
    [Key]
    public int PrescriptionId { get; set; }

    public int CustomerId { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? RightSPH { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? RightCYL { get; set; }

    public int? RightAXIS { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? LeftSPH { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? LeftCYL { get; set; }

    public int? LeftAXIS { get; set; }

    [Column(TypeName = "decimal(4,1)")]
    public decimal? PD { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; } = null!;
}
