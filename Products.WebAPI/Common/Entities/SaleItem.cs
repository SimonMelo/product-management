using Products.WebAPI.Common.Enums;

namespace Products.WebAPI.Common.Entities;

public class SaleItem
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public string ProductBarcode { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    
    public Sale? Sale { get; set; }
    public Products ? Product { get; set; }
}