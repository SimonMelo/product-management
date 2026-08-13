using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Sales.GetSales;

public class GetSalesHandler(AppDbContext db)
    : IRequestHandler<GetSalesQuery, Result<List<SaleListItemResponse>>>
{
    public async Task<Result<List<SaleListItemResponse>>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var sales = await db.Sales
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Items)
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new SaleListItemResponse(
                s.Id,
                s.TotalAmount,
                s.PaymentMethod,
                s.CustomerName,
                s.User!.Name,
                s.Items.Count,
                s.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        return Result.Ok(sales);
    }
}
