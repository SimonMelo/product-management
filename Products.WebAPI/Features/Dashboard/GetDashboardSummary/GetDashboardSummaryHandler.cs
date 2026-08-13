using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Dashboard.GetDashboardSummary;

public class GetDashboardSummaryHandler(AppDbContext db)
    : IRequestHandler<GetDashboardSummaryQuery, Result<DashboardSummaryResponse>>
{
    public async Task<Result<DashboardSummaryResponse>> Handle(GetDashboardSummaryQuery query, CancellationToken cancellationToken)
    {
        var products = await db.Products
            .AsNoTracking()
            .Select(p => new
            {
                p.Disp,
                Stock = db.StockMovements
                    .Where(m => m.ProductBarcode == p.Barcode)
                    .Sum(m => (int?)m.Quantity) ?? 0
            })
            .ToListAsync(cancellationToken);

        var total = products.Count;
        var esgotados = products.Count(p => p.Stock <= 0);
        var disponiveis = products.Count(p => p.Stock > 0 && p.Disp);
        var emEstoque = products.Count(p => p.Stock > 0 && !p.Disp);

        return Result<DashboardSummaryResponse>.Ok(new DashboardSummaryResponse(total, disponiveis, emEstoque, esgotados));
    }
}
