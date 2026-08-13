using MediatR;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Features.StockMovement.RegisterStockIn;

namespace Products.WebAPI.Features.StockMovement.RegisterAdjustment;

public record RegisterAdjustmentCommand(string Barcode, int Quantity, int UserId) : IRequest<Result<StockMovementResponse>>;
