using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Products.UpdateProduct;

public record UpdateProductCommand(
    string Barcode,
    string Name,
    int CategoryId,
    int BrandId,
    bool Disp,
    decimal Price
) : IRequest<Result<UpdateProductResponse>>;

public record UpdateProductResponse(string Barcode, string Name, string Brand, string Category);
