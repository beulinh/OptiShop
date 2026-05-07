using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptiShop.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required, MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }

    [MaxLength(100)]
    public string? Material { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,0)")]
    public decimal Price { get; set; }

    public int Stock { get; set; } = 0;

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    [ForeignKey("BrandId")]
    public Brand? Brand { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();
}
