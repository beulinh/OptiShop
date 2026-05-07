/*
 * ============================================================
 * Tên file   : Brand.cs
 * Tác giả    : Hoàng Nhật Huy
 * Ngày tạo   : 07/05/2026
 * Mục đích   :
 *  - Khai báo model Brand trong hệ thống OptiShop.
 *  - Quản lý thông tin thương hiệu sản phẩm như:
 *      + Tên thương hiệu
 *      + Quốc gia
 *  - Thiết lập quan hệ giữa Brand và Product.
 *  - Một Brand có thể chứa nhiều Product.
 *  - Sử dụng Entity Framework Core để ánh xạ dữ liệu
 *    sang bảng Brands trong cơ sở dữ liệu.
 * ============================================================
 */

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