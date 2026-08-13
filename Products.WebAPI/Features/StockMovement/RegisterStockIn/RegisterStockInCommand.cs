using MediatR;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.StockMovement.RegisterStockIn;

public record RegisterStockInCommand(List<StockInItemRequest> Items, int UserId) : IRequest<Result<List<StockMovementResponse>>>;

public record StockMovementResponse(
    int Id,
    string ProductBarcode,
    string ProductName,
    int Quantity,
    string Type,
    int NewStock,
    DateTime CreatedAt
);