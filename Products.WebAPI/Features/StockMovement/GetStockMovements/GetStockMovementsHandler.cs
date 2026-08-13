using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.StockMovement.GetStockMovements;

public class GetStockMovementsHandler(AppDbContext db)
    : IRequestHandler<GetStockMovementsQuery, Result<List<GetStockMovementsResponse>>>
{
    public async Task<Result<List<GetStockMovementsResponse>>> Handle(GetStockMovementsQuery query, CancellationToken cancellationToken)
    {
        var movementsQuery = db.StockMovements
            .AsNoTracking()
            .Include(m => m.Product)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.ProductBarcode))
            movementsQuery = movementsQuery.Where(m => m.ProductBarcode == query.ProductBarcode);

        var movements = await movementsQuery
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new GetStockMovementsResponse(
                m.Id,
                m.ProductBarcode,
                m.Product!.Name,
                m.Quantity,
                m.Type.ToString(),
                m.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        return Result.Ok(movements);
    }
}
