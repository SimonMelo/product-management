namespace Products.WebAPI.Common.Entities;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Products> Products { get; set; } = [];
}