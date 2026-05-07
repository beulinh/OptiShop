using OptiShop.Models;

namespace OptiShop.DataAccess.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Customer> Customers { get; }
    IRepository<Prescription> Prescriptions { get; }
    IRepository<Category> Categories { get; }
    IRepository<Brand> Brands { get; }
    IRepository<Product> Products { get; }
    IRepository<Order> Orders { get; }
    IRepository<OrderDetail> OrderDetails { get; }
    IRepository<Supplier> Suppliers { get; }
    IRepository<ImportReceipt> ImportReceipts { get; }
    IRepository<ImportDetail> ImportDetails { get; }
    Task<int> SaveChangesAsync();
}
