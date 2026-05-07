using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptiShop.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    public int CustomerId { get; set; }
    public int? UserId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required, MaxLength(30)]
    public string Status { get; set; } = "DangXuLy";

    [Required, MaxLength(20)]
    public string OrderType { get; set; } = "TaiQuay";

    [Column(TypeName = "decimal(18,0)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(255)]
    public string? ShippingAddress { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    // Navigation
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; } = null!;

    [ForeignKey("UserId")]
    public User? User { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
