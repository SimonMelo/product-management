using Products.WebAPI.Common.Enums;

namespace Products.WebAPI.Common.Entities;

public class StockMovement
{
    public int Id { get; set; }
    public string ProductBarcode { get; set; } = null!;
    public int Quantity { get; set; }
    public EMovementType Type { get; set; }
    public int UserId { get; set; }
    public int? SaleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Products? Product { get; set; }
    public User? User { get; set; }
    public Sale? Sale { get; set; }
}