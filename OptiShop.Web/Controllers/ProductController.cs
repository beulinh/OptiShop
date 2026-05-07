using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OptiShop.Business.Interfaces;
using OptiShop.DataAccess.Repositories;
using OptiShop.Models;

namespace OptiShop.Web.Controllers;

[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _uow;

    public ProductController(IProductService productService, IUnitOfWork uow)
    {
        _productService = productService;
        _uow = uow;
    }

    public async Task<IActionResult> Index(string? keyword, int? categoryId, int? brandId)
    {
        var products = await _productService.SearchAsync(keyword, categoryId, brandId);
        await LoadDropdowns();
        ViewBag.Keyword = keyword;
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        await LoadDropdowns();
        return View(new Product());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var path = Path.Combine("wwwroot/images/products", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            using var stream = new FileStream(path, FileMode.Create);
            await imageFile.CopyToAsync(stream);
            product.ImageUrl = "/images/products/" + fileName;
        }
        await _productService.CreateAsync(product);
        TempData["Success"] = "Thêm sản phẩm thành công!";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        await LoadDropdowns();
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Product product, IFormFile? imageFile)
    {
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

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        TempData["Success"] = "Xóa sản phẩm thành công!";
        return RedirectToAction("Index");
    }

    private async Task LoadDropdowns()
    {
        ViewBag.Categories = new SelectList(await _uow.Categories.GetAllAsync(), "CategoryId", "CategoryName");
        ViewBag.Brands = new SelectList(await _uow.Brands.GetAllAsync(), "BrandId", "BrandName");
    }
}
