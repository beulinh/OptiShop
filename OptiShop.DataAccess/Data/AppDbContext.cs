using Microsoft.EntityFrameworkCore;
using OptiShop.Models;

namespace OptiShop.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<ImportReceipt> ImportReceipts => Set<ImportReceipt>();
    public DbSet<ImportDetail> ImportDetails => Set<ImportDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique constraints
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique().HasFilter("[Email] IS NOT NULL");
        modelBuilder.Entity<Category>().HasIndex(c => c.CategoryName).IsUnique();
        modelBuilder.Entity<Brand>().HasIndex(b => b.BrandName).IsUnique();

        // Check constraints
        modelBuilder.Entity<Prescription>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_Prescription_RightAXIS", "[RightAXIS] BETWEEN 0 AND 180");
            t.HasCheckConstraint("CK_Prescription_LeftAXIS", "[LeftAXIS] BETWEEN 0 AND 180");
        });

        modelBuilder.Entity<Product>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_Product_Price", "[Price] > 0");
            t.HasCheckConstraint("CK_Product_Stock", "[Stock] >= 0");
        });

        modelBuilder.Entity<OrderDetail>().ToTable(t =>
            t.HasCheckConstraint("CK_OrderDetail_Quantity", "[Quantity] > 0"));

        modelBuilder.Entity<ImportDetail>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_ImportDetail_Quantity", "[Quantity] > 0");
            t.HasCheckConstraint("CK_ImportDetail_UnitPrice", "[UnitPrice] > 0");
        });

        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                FullName = "Quản trị viên",
                Email = "admin@optishop.vn",
                Role = "Admin",
                IsActive = true,
                CreatedDate = new DateTime(2026, 1, 1)
            },
            new User
            {
                UserId = 2,
                Username = "nvbanhang",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                FullName = "Nguyễn Văn Bán",
                Email = "banhang@optishop.vn",
                Role = "NVBanHang",
                IsActive = true,
                CreatedDate = new DateTime(2026, 1, 1)
            },
            new User
            {
                UserId = 3,
                Username = "nvkho",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                FullName = "Trần Thị Kho",
                Email = "kho@optishop.vn",
                Role = "NVKho",
                IsActive = true,
                CreatedDate = new DateTime(2026, 1, 1)
            }
        );

        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, CategoryName = "Gọng kính", Description = "Các loại gọng kính thời trang và y tế" },
            new Category { CategoryId = 2, CategoryName = "Tròng kính", Description = "Tròng kính cận, viễn, loạn, đa tròng" },
            new Category { CategoryId = 3, CategoryName = "Kính râm", Description = "Kính mát chống tia UV" },
            new Category { CategoryId = 4, CategoryName = "Phụ kiện", Description = "Hộp kính, khăn lau, nước rửa kính" }
        );

        modelBuilder.Entity<Brand>().HasData(
            new Brand { BrandId = 1, BrandName = "Ray-Ban", Country = "Ý" },
            new Brand { BrandId = 2, BrandName = "Oakley", Country = "Mỹ" },
            new Brand { BrandId = 3, BrandName = "Essilor", Country = "Pháp" },
            new Brand { BrandId = 4, BrandName = "Chemi", Country = "Hàn Quốc" },
            new Brand { BrandId = 5, BrandName = "Molsion", Country = "Trung Quốc" }
        );
    }
}
