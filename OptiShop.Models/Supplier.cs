/*
 * ============================================================
 * File        : Supplier.cs
 * Người viết  : Bùi Huy Bình
 * Ngày tạo    : 07/05/2026
 * ------------------------------------------------------------
 * Mô tả:
 *  Class Supplier dùng để lưu trữ và quản lý thông tin
 *  nhà cung cấp trong hệ thống OptiShop.
 *
 *  Chức năng chính:
 *   - Lưu tên nhà cung cấp
 *   - Lưu thông tin liên hệ
 *   - Quản lý trạng thái hoạt động
 *   - Liên kết với các phiếu nhập hàng
 *
 *  Quan hệ dữ liệu:
 *   - Một nhà cung cấp có thể tạo nhiều phiếu nhập.
 *
 *  Công nghệ sử dụng:
 *   - Entity Framework Core
 *   - Data Annotations
 * ============================================================
 */

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