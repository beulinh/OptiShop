namespace OptiShop.Business.DTOs;

public class LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    public string? Material { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
}

public class OrderCreateDto
{
    public int CustomerId { get; set; }
    public string OrderType { get; set; } = "TaiQuay";
    public string? ShippingAddress { get; set; }
    public string? Note { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int? PrescriptionId { get; set; }
}

public class ImportCreateDto
{
    public int SupplierId { get; set; }
    public string? Note { get; set; }
    public List<ImportItemDto> Items { get; set; } = new();
}

public class ImportItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class DashboardDto
{
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
}
