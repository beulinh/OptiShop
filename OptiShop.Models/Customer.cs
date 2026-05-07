using System.ComponentModel.DataAnnotations;

namespace OptiShop.Models;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(15)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(255)]
    public string? Address { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
