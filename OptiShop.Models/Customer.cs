/*
 * ============================================================
 * Tên file   : Customer.cs
 * Tác giả    : Hoàng Nhật Huy
 * Ngày tạo   : 07/05/2026
 * Mục đích   :
 *  - Khai báo model Customer trong hệ thống OptiShop.
 *  - Quản lý thông tin khách hàng như:
 *      + Họ tên
 *      + Số điện thoại
 *      + Email
 *      + Địa chỉ
 *      + Ngày tạo
 *  - Thiết lập quan hệ giữa Customer với:
 *      + Prescription (Đơn thuốc)
 *      + Order (Đơn hàng)
 *  - Sử dụng Entity Framework Core để ánh xạ dữ liệu
 *    sang bảng Customers trong cơ sở dữ liệu.
 * ============================================================
 */

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