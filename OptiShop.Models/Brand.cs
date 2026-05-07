using System.ComponentModel.DataAnnotations;

namespace OptiShop.Models;

public class Brand
{
    [Key]
    public int BrandId { get; set; }

    [Required, MaxLength(100)]
    public string BrandName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Country { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
