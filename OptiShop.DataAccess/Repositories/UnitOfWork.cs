using OptiShop.DataAccess.Data;
using OptiShop.Models;

namespace OptiShop.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new Repository<User>(context);
        Customers = new Repository<Customer>(context);
        Prescriptions = new Repository<Prescription>(context);
        Categories = new Repository<Category>(context);
        Brands = new Repository<Brand>(context);
        Products = new Repository<Product>(context);
        Orders = new Repository<Order>(context);
        OrderDetails = new Repository<OrderDetail>(context);
        Suppliers = new Repository<Supplier>(context);
        ImportReceipts = new Repository<ImportReceipt>(context);
        ImportDetails = new Repository<ImportDetail>(context);
    }

    public IRepository<User> Users { get; }
    public IRepository<Customer> Customers { get; }
    public IRepository<Prescription> Prescriptions { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<Brand> Brands { get; }
    public IRepository<Product> Products { get; }
    public IRepository<Order> Orders { get; }
    public IRepository<OrderDetail> OrderDetails { get; }
    public IRepository<Supplier> Suppliers { get; }
    public IRepository<ImportReceipt> ImportReceipts { get; }
    public IRepository<ImportDetail> ImportDetails { get; }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
