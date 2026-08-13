using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Products.GetProductByBarcode;

public record GetProductByBarcodeQuery(string Barcode) : IRequest<Result<GetProductByBarcodeResponse>>;

public record GetProductByBarcodeResponse(
    string Barcode,
    string Name,
    int CategoryId,
    string Category,
    int BrandId,
    string Brand,
    decimal Price,
    bool Disp,
    int Stock,
    string Status,
    string? VirtualPath,
    List<BrandOptionResponse> Brands,
    List<CategoryOptionResponse> Categories,
    List<ProductMovementResponse> Movements);

public record BrandOptionResponse(int Id, string Name);

public record CategoryOptionResponse(int Id, string Name);

public record ProductMovementResponse(
    int Id,
    string ProductBarcode,
    string ProductName,
    int Quantity,
    string Type,
    DateTime CreatedAt);