namespace Products.WebAPI.Common.Entities;

public class Products
{
    public string Barcode { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public string? VirtualPath { get; set; }
    public int UserId { get; set; }
    public bool Disp { get; set; } = true;
    public decimal Price { get; set; } = 10.00m;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Category? Category { get; set; }
    public Brand? Brand { get; set; }
    public User? User { get; set; }
    public ICollection<StockMovement> StockMovements { get; set; } = [];
}