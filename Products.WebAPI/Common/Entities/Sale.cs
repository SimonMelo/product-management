using Products.WebAPI.Common.Enums;

namespace Products.WebAPI.Common.Entities;

public class Sale
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public EPaymentMethod PaymentMethod { get; set; }
    public int UserId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public User? User { get; set; }
    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
}