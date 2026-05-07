using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OptiShop.Business.DTOs;
using OptiShop.Business.Interfaces;
using OptiShop.DataAccess.Repositories;
using OptiShop.Models;

namespace OptiShop.Web.Controllers;

[Authorize(Roles = "Admin,NVBanHang")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;
    private readonly IUnitOfWork _uow;

    public OrderController(IOrderService orderService, IProductService productService, IUnitOfWork uow)
    {
        _orderService = orderService;
        _productService = productService;
        _uow = uow;
    }

    public async Task<IActionResult> Index(string? status)
    {
        var orders = string.IsNullOrEmpty(status) ? await _orderService.GetAllAsync() : await _orderService.GetByStatusAsync(status);
        ViewBag.CurrentStatus = status;
        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();
        return View(order);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Customers = new SelectList(await _uow.Customers.GetAllAsync(), "CustomerId", "FullName");
        ViewBag.Products = await _productService.GetAllAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateDto dto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var order = await _orderService.CreateOrderAsync(userId, dto);
            TempData["Success"] = $"Tạo đơn hàng #{order.OrderId} thành công!";
            return RedirectToAction("Details", new { id = order.OrderId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Create");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        await _orderService.UpdateStatusAsync(id, status);
        TempData["Success"] = "Cập nhật trạng thái thành công!";
        return RedirectToAction("Details", new { id });
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        await _orderService.CancelAsync(id);
        TempData["Success"] = "Đã hủy đơn hàng!";
        return RedirectToAction("Index");
    }
}

[Authorize(Roles = "Admin,NVBanHang")]
public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService) => _customerService = customerService;

    public async Task<IActionResult> Index(string? keyword)
    {
        var customers = string.IsNullOrEmpty(keyword) ? await _customerService.GetAllAsync() : await _customerService.SearchAsync(keyword);
        ViewBag.Keyword = keyword;
        return View(customers);
    }

    public IActionResult Create() => View(new Customer());

    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        await _customerService.CreateAsync(customer);
        TempData["Success"] = "Thêm khách hàng thành công!";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return View(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Customer customer)
    {
        await _customerService.UpdateAsync(customer);
        TempData["Success"] = "Cập nhật thành công!";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return View(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _customerService.DeleteAsync(id);
        TempData["Success"] = "Xóa khách hàng thành công!";
        return RedirectToAction("Index");
    }
}

[Authorize(Roles = "Admin,NVKho")]
public class WarehouseController : Controller
{
    private readonly IWarehouseService _warehouseService;
    private readonly IProductService _productService;
    private readonly IUnitOfWork _uow;

    public WarehouseController(IWarehouseService warehouseService, IProductService productService, IUnitOfWork uow)
    {
        _warehouseService = warehouseService;
        _productService = productService;
        _uow = uow;
    }

    public async Task<IActionResult> Index()
    {
        var receipts = await _warehouseService.GetAllAsync();
        return View(receipts);
    }

    public async Task<IActionResult> Details(int id)
    {
        var receipt = await _warehouseService.GetByIdAsync(id);
        if (receipt == null) return NotFound();
        return View(receipt);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Suppliers = new SelectList(await _uow.Suppliers.FindAsync(s => s.IsActive), "SupplierId", "SupplierName");
        ViewBag.Products = await _productService.GetAllAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ImportCreateDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var receipt = await _warehouseService.CreateImportAsync(userId, dto);
        TempData["Success"] = $"Tạo phiếu nhập #{receipt.ImportId} thành công!";
        return RedirectToAction("Details", new { id = receipt.ImportId });
    }
}

[Authorize(Roles = "Admin")]
public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;
    public SupplierController(ISupplierService supplierService) => _supplierService = supplierService;

    public async Task<IActionResult> Index()
    {
        var suppliers = await _supplierService.GetAllAsync();
        return View(suppliers);
    }

    public IActionResult Create() => View(new Supplier());

    [HttpPost]
    public async Task<IActionResult> Create(Supplier supplier)
    {
        await _supplierService.CreateAsync(supplier);
        TempData["Success"] = "Thêm nhà cung cấp thành công!";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);
        if (supplier == null) return NotFound();
        return View(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Supplier supplier)
    {
        await _supplierService.UpdateAsync(supplier);
        TempData["Success"] = "Cập nhật thành công!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _supplierService.DeleteAsync(id);
        TempData["Success"] = "Xóa nhà cung cấp thành công!";
        return RedirectToAction("Index");
    }
}

public class ShopController : Controller
{
    private readonly IProductService _productService;
    public ShopController(IProductService productService) => _productService = productService;

    public async Task<IActionResult> Index(string? keyword, int? categoryId, int? brandId)
    {
        var products = await _productService.SearchAsync(keyword, categoryId, brandId);
        return View(products.Where(p => p.IsActive && p.Stock > 0));
    }
}
