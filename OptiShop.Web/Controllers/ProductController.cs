/*
 * ============================================================
 * File        : ProductController.cs
 * Người viết  : Linh
 * Ngày tạo    : 07/05/2026
 * ------------------------------------------------------------
 * Mô tả:
 *  Controller quản lý sản phẩm trong hệ thống OptiShop.
 *  
 *  Chức năng:
 *   - Hiển thị danh sách sản phẩm
 *   - Thêm / sửa / xóa sản phẩm
 *   - Tìm kiếm sản phẩm
 *   - Upload ảnh sản phẩm
 * ============================================================
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OptiShop.Business.Interfaces;
using OptiShop.DataAccess.Repositories;
using OptiShop.Models;

namespace OptiShop.Web.Controllers;

/*
 * Controller xử lý các chức năng liên quan đến Product
 */
[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _uow;

    // Constructor inject service và repository
    public ProductController(IProductService productService, IUnitOfWork uow)
    {
        _productService = productService;
        _uow = uow;
    }

    // Hiển thị danh sách và tìm kiếm sản phẩm
    public async Task<IActionResult> Index(string? keyword, int? categoryId, int? brandId)
    {
        var products = await _productService.SearchAsync(keyword, categoryId, brandId);

        await LoadDropdowns();

        ViewBag.Keyword = keyword;

        return View(products);
    }

    // Hiển thị form thêm sản phẩm
    public async Task<IActionResult> Create()
    {
        await LoadDropdowns();
        return View(new Product());
    }

    // Xử lý thêm sản phẩm mới
    [HttpPost]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        // Kiểm tra upload ảnh
        if (imageFile != null && imageFile.Length > 0)
        {
            // Tạo tên file ngẫu nhiên tránh trùng
            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);

            var path = Path.Combine("wwwroot/images/products", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            // Lưu ảnh vào thư mục server
            using var stream = new FileStream(path, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            product.ImageUrl = "/images/products/" + fileName;
        }

        await _productService.CreateAsync(product);

        TempData["Success"] = "Thêm sản phẩm thành công!";

        return RedirectToAction("Index");
    }

    // Hiển thị form cập nhật sản phẩm
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        await LoadDropdowns();

        return View(product);
    }

    // Xử lý cập nhật sản phẩm
    [HttpPost]
    public async Task<IActionResult> Edit(Product product, IFormFile? imageFile)
    {
        // Nếu có upload ảnh mới
        if (imageFile != null && imageFile.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);

            var path = Path.Combine("wwwroot/images/products", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            using var stream = new FileStream(path, FileMode.Create);

            await imageFile.CopyToAsync(stream);

            product.ImageUrl = "/images/products/" + fileName;
        }

        await _productService.UpdateAsync(product);

        TempData["Success"] = "Cập nhật sản phẩm thành công!";

        return RedirectToAction("Index");
    }

    // Xóa sản phẩm
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);

        TempData["Success"] = "Xóa sản phẩm thành công!";

        return RedirectToAction("Index");
    }

    // Load dữ liệu cho dropdown Category và Brand
    private async Task LoadDropdowns()
    {
        ViewBag.Categories =
            new SelectList(await _uow.Categories.GetAllAsync(),
                "CategoryId", "CategoryName");

        ViewBag.Brands =
            new SelectList(await _uow.Brands.GetAllAsync(),
                "BrandId", "BrandName");
    }
}