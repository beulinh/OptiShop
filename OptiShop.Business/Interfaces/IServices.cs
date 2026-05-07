using OptiShop.Models;

namespace OptiShop.Business.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> SearchAsync(string? keyword, int? categoryId, int? brandId);
    Task CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task<bool> CheckStockAsync(int productId, int quantity);
}

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task<IEnumerable<Customer>> SearchAsync(string keyword);
    Task CreateAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetByStatusAsync(string status);
    Task<Order> CreateOrderAsync(int userId, Business.DTOs.OrderCreateDto dto);
    Task UpdateStatusAsync(int orderId, string status);
    Task CancelAsync(int orderId);
}

public interface IWarehouseService
{
    Task<IEnumerable<ImportReceipt>> GetAllAsync();
    Task<ImportReceipt?> GetByIdAsync(int id);
    Task<ImportReceipt> CreateImportAsync(int userId, Business.DTOs.ImportCreateDto dto);
}

public interface ISupplierService
{
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<Supplier?> GetByIdAsync(int id);
    Task CreateAsync(Supplier supplier);
    Task UpdateAsync(Supplier supplier);
    Task DeleteAsync(int id);
}

public interface IReportService
{
    Task<Business.DTOs.DashboardDto> GetDashboardAsync();
    Task<decimal> GetRevenueAsync(DateTime from, DateTime to);
    Task<IEnumerable<Product>> GetTopSellingAsync(int top, DateTime from, DateTime to);
}
