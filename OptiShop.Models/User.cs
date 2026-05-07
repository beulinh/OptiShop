using System.ComponentModel.DataAnnotations;

namespace OptiShop.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(15)]
    public string? Phone { get; set; }

    [Required, MaxLength(20)]
    public string Role { get; set; } = "KhachHang";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<ImportReceipt> ImportReceipts { get; set; } = new List<ImportReceipt>();
}
