namespace Products.WebAPI.Features.Products.GetProducts;

public record GetProductResponse(string Barcode, string Name, string Category, string Brand, string? VirtualPath, bool Disp, decimal Price, int Stock, string Status);
