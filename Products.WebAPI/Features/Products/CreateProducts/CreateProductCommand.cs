using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Products.CreateProducts;

public record CreateProductCommand(string Barcode, string Name, int CategoryId, int BrandId, bool Disp, decimal Price) : IRequest<Result<ProductResponse>>;

public record ProductResponse(string Barcode, string Name, string Brand, string Category);