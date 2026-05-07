/*
 * ============================================================
 * File        : ImportDetail.cs
 * Người viết  : Bùi Huy Bình
 * Ngày tạo    : 07/05/2026
 * ------------------------------------------------------------
 * Mô tả:
 *  Class ImportDetail đại diện cho chi tiết phiếu nhập
 *  trong hệ thống quản lý OptiShop.
 *
 *  Chức năng:
 *   - Lưu thông tin sản phẩm được nhập kho
 *   - Quản lý số lượng nhập
 *   - Lưu đơn giá nhập của từng sản phẩm
 *   - Liên kết giữa phiếu nhập và sản phẩm
 *
 *  Quan hệ:
 *   - Một ImportReceipt có nhiều ImportDetail
 *   - Một Product có thể xuất hiện trong nhiều ImportDetail
 *
 *  Công nghệ sử dụng:
 *   - Entity Framework Core
 *   - Data Annotations
 * ============================================================
 */

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