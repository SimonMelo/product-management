using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Features.StockMovement.RegisterStockIn;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.StockMovement.RegisterAdjustment;

public class RegisterAdjustmentHandler(AppDbContext db)
    : IRequestHandler<RegisterAdjustmentCommand, Result<StockMovementResponse>>
{
    public async Task<Result<StockMovementResponse>> Handle(RegisterAdjustmentCommand command, CancellationToken cancellationToken)
    {
        var product = await db.Products.FindAsync([command.Barcode], cancellationToken);
        if (product is null)
            return Result.Fail<StockMovementResponse>($"Produto não encontrado: {command.Barcode}");

        var movement = new Common.Entities.StockMovement
        {
            ProductBarcode = command.Barcode,
            Quantity = command.Quantity,
            Type = EMovementType.Ajuste,
            UserId = command.UserId,
            CreatedAt = DateTime.UtcNow
        };

        db.StockMovements.Add(movement);
        await db.SaveChangesAsync(cancellationToken);

        var newStock = await db.StockMovements
            .Where(m => m.ProductBarcode == command.Barcode)
            .SumAsync(m => m.Quantity, cancellationToken);

        var response = new StockMovementResponse(
            movement.Id,
            movement.ProductBarcode,
            product.Name,
            movement.Quantity,
            movement.Type.ToString(),
            newStock,
            movement.CreatedAt
        );

        return Result.Ok(response);
    }
}
