using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.StockMovement.GetStockMovements;

public record GetStockMovementsQuery(string? ProductBarcode) : IRequest<Result<List<GetStockMovementsResponse>>>;

public record GetStockMovementsResponse(
    int Id,
    string ProductBarcode,
    string ProductName,
    int Quantity,
    string Type,
    DateTime CreatedAt
);
