using Microsoft.EntityFrameworkCore;
using OptiShop.Business.DTOs;
using OptiShop.Business.Interfaces;
using OptiShop.DataAccess.Repositories;
using OptiShop.Models;

namespace OptiShop.Business.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uow;
    public ProductService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await _uow.Products.Query().Include(p => p.Category).Include(p => p.Brand).ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await _uow.Products.Query().Include(p => p.Category).Include(p => p.Brand).FirstOrDefaultAsync(p => p.ProductId == id);

    public async Task<IEnumerable<Product>> SearchAsync(string? keyword, int? categoryId, int? brandId)
    {
        var query = _uow.Products.Query().Include(p => p.Category).Include(p => p.Brand).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(p => p.ProductName.Contains(keyword));
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);
        if (brandId.HasValue)
            query = query.Where(p => p.BrandId == brandId);
        return await query.ToListAsync();
    }

    public async Task CreateAsync(Product product) { await _uow.Products.AddAsync(product); await _uow.SaveChangesAsync(); }
    public async Task UpdateAsync(Product product) { _uow.Products.Update(product); await _uow.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var p = await _uow.Products.GetByIdAsync(id);
        if (p != null) { p.IsActive = false; await _uow.SaveChangesAsync(); }
    }
    public async Task<bool> CheckStockAsync(int productId, int quantity)
    {
        var p = await _uow.Products.GetByIdAsync(productId);
        return p != null && p.Stock >= quantity;
    }
}

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _uow;
    public CustomerService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<Customer>> GetAllAsync() => await _uow.Customers.GetAllAsync();
    public async Task<Customer?> GetByIdAsync(int id)
        => await _uow.Customers.Query().Include(c => c.Prescriptions).Include(c => c.Orders).FirstOrDefaultAsync(c => c.CustomerId == id);

    public async Task<IEnumerable<Customer>> SearchAsync(string keyword)
        => await _uow.Customers.FindAsync(c => c.FullName.Contains(keyword) || c.Phone.Contains(keyword));

    public async Task CreateAsync(Customer customer) { await _uow.Customers.AddAsync(customer); await _uow.SaveChangesAsync(); }
    public async Task UpdateAsync(Customer customer) { _uow.Customers.Update(customer); await _uow.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var c = await _uow.Customers.GetByIdAsync(id);
        if (c != null) { _uow.Customers.Remove(c); await _uow.SaveChangesAsync(); }
    }
}

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uow;
    public OrderService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<Order>> GetAllAsync()
        => await _uow.Orders.Query().Include(o => o.Customer).Include(o => o.User).OrderByDescending(o => o.OrderDate).ToListAsync();

    public async Task<Order?> GetByIdAsync(int id)
        => await _uow.Orders.Query().Include(o => o.Customer).Include(o => o.OrderDetails).ThenInclude(d => d.Product).FirstOrDefaultAsync(o => o.OrderId == id);

    public async Task<IEnumerable<Order>> GetByStatusAsync(string status)
        => await _uow.Orders.FindAsync(o => o.Status == status);

    public async Task<Order> CreateOrderAsync(int userId, OrderCreateDto dto)
    {
        var order = new Order
        {
            CustomerId = dto.CustomerId,
            UserId = userId,
            OrderType = dto.OrderType,
            ShippingAddress = dto.ShippingAddress,
            Note = dto.Note,
            Status = dto.OrderType == "TrucTuyen" ? "ChoXacNhan" : "DangXuLy"
        };

        decimal total = 0;
        foreach (var item in dto.Items)
        {
            var product = await _uow.Products.GetByIdAsync(item.ProductId);
            if (product == null || product.Stock < item.Quantity)
                throw new Exception($"Sản phẩm {item.ProductId} không đủ hàng");

            order.OrderDetails.Add(new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                PrescriptionId = item.PrescriptionId
            });
            total += product.Price * item.Quantity;
            product.Stock -= item.Quantity;
        }
        order.TotalAmount = total;
        await _uow.Orders.AddAsync(order);
        await _uow.SaveChangesAsync();
        return order;
    }

    public async Task UpdateStatusAsync(int orderId, string status)
    {
        var order = await _uow.Orders.GetByIdAsync(orderId);
        if (order != null) { order.Status = status; await _uow.SaveChangesAsync(); }
    }

    public async Task CancelAsync(int orderId)
    {
        var order = await _uow.Orders.Query().Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order != null && order.Status != "DaGiao" && order.Status != "HoanThanh")
        {
            foreach (var detail in order.OrderDetails)
            {
                var product = await _uow.Products.GetByIdAsync(detail.ProductId);
                if (product != null) product.Stock += detail.Quantity;
            }
            order.Status = "DaHuy";
            await _uow.SaveChangesAsync();
        }
    }
}

public class WarehouseService : IWarehouseService
{
    private readonly IUnitOfWork _uow;
    public WarehouseService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<ImportReceipt>> GetAllAsync()
        => await _uow.ImportReceipts.Query().Include(i => i.Supplier).Include(i => i.User).OrderByDescending(i => i.ImportDate).ToListAsync();

    public async Task<ImportReceipt?> GetByIdAsync(int id)
        => await _uow.ImportReceipts.Query().Include(i => i.Supplier).Include(i => i.ImportDetails).ThenInclude(d => d.Product).FirstOrDefaultAsync(i => i.ImportId == id);

    public async Task<ImportReceipt> CreateImportAsync(int userId, ImportCreateDto dto)
    {
        var receipt = new ImportReceipt { SupplierId = dto.SupplierId, UserId = userId, Note = dto.Note };
        decimal total = 0;
        foreach (var item in dto.Items)
        {
            receipt.ImportDetails.Add(new ImportDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });
            total += item.UnitPrice * item.Quantity;

            var product = await _uow.Products.GetByIdAsync(item.ProductId);
            if (product != null) product.Stock += item.Quantity;
        }
        receipt.TotalAmount = total;
        await _uow.ImportReceipts.AddAsync(receipt);
        await _uow.SaveChangesAsync();
        return receipt;
    }
}

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _uow;
    public SupplierService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<Supplier>> GetAllAsync() => await _uow.Suppliers.FindAsync(s => s.IsActive);
    public async Task<Supplier?> GetByIdAsync(int id) => await _uow.Suppliers.GetByIdAsync(id);
    public async Task CreateAsync(Supplier supplier) { await _uow.Suppliers.AddAsync(supplier); await _uow.SaveChangesAsync(); }
    public async Task UpdateAsync(Supplier supplier) { _uow.Suppliers.Update(supplier); await _uow.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var s = await _uow.Suppliers.GetByIdAsync(id);
        if (s != null) { s.IsActive = false; await _uow.SaveChangesAsync(); }
    }
}

public class ReportService : IReportService
{
    private readonly IUnitOfWork _uow;
    public ReportService(IUnitOfWork uow) => _uow = uow;

    public async Task<DashboardDto> GetDashboardAsync() => new DashboardDto
    {
        TotalRevenue = await _uow.Orders.Query().Where(o => o.Status == "HoanThanh").SumAsync(o => o.TotalAmount),
        TotalOrders = await _uow.Orders.CountAsync(),
        TotalProducts = await _uow.Products.CountAsync(p => p.IsActive),
        TotalCustomers = await _uow.Customers.CountAsync()
    };

    public async Task<decimal> GetRevenueAsync(DateTime from, DateTime to)
        => await _uow.Orders.Query().Where(o => o.Status == "HoanThanh" && o.OrderDate >= from && o.OrderDate <= to).SumAsync(o => o.TotalAmount);

    public async Task<IEnumerable<Product>> GetTopSellingAsync(int top, DateTime from, DateTime to)
        => await _uow.OrderDetails.Query()
            .Where(d => d.Order.OrderDate >= from && d.Order.OrderDate <= to && d.Order.Status == "HoanThanh")
            .GroupBy(d => d.ProductId)
            .OrderByDescending(g => g.Sum(d => d.Quantity))
            .Take(top)
            .Select(g => g.First().Product)
            .ToListAsync();
}
