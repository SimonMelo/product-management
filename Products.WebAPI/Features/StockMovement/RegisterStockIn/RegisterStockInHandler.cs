using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.StockMovement.RegisterStockIn;

public class RegisterStockInHandler(AppDbContext db)
    : IRequestHandler<RegisterStockInCommand, Result<List<StockMovementResponse>>>
{
    public async Task<Result<List<StockMovementResponse>>> Handle(RegisterStockInCommand command, CancellationToken cancellationToken)
    {
        var groupedItems = command.Items
            .GroupBy(i => i.Barcode)
            .Select(g => new StockInItemRequest(g.Key, g.Sum(i => i.Quantity)))
            .ToList();

        var barcodes = groupedItems.Select(i => i.Barcode).ToList();

        var products = await db.Products
            .Where(p => barcodes.Contains(p.Barcode))
            .ToDictionaryAsync(p => p.Barcode, cancellationToken);

        var missingBarcodes = barcodes.Where(b => !products.ContainsKey(b)).ToList();
        if (missingBarcodes.Count != 0)
            return Result.Fail<List<StockMovementResponse>>(
                $"Produto(s) não encontrado(s): {string.Join(", ", missingBarcodes)}");

        var movements = groupedItems.Select(item => new Common.Entities.StockMovement()
        {
            ProductBarcode = item.Barcode,
            Quantity = Math.Abs(item.Quantity),
            Type = EMovementType.Entrada,
            UserId = command.UserId,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        db.StockMovements.AddRange(movements);
        await db.SaveChangesAsync(cancellationToken);

        var updatedStocks = await db.StockMovements
            .Where(m => barcodes.Contains(m.ProductBarcode))
            .GroupBy(m => m.ProductBarcode)
            .Select(g => new { Barcode = g.Key, Stock = g.Sum(m => m.Quantity) })
            .ToDictionaryAsync(x => x.Barcode, x => x.Stock, cancellationToken);

        var response = movements.Select(m => new StockMovementResponse(
            m.Id,
            m.ProductBarcode,
            products[m.ProductBarcode].Name,
            m.Quantity,
            m.Type.ToString(),
            updatedStocks[m.ProductBarcode],
            m.CreatedAt
        )).ToList();

        return Result.Ok(response);
    }
}