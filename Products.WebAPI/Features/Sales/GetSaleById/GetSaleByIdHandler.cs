using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Sales.GetSaleById;

public class GetSaleByIdHandler(AppDbContext db)
    : IRequestHandler<GetSaleByIdQuery, Result<SaleDetailResponse>>
{
    public async Task<Result<SaleDetailResponse>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await db.Sales
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (sale is null)
            return Result.Fail<SaleDetailResponse>("Venda não encontrada.");

        var barcodes = sale.Items.Select(i => i.ProductBarcode).ToList();

        var productNames = await db.Products
            .Where(p => barcodes.Contains(p.Barcode))
            .ToDictionaryAsync(p => p.Barcode, p => p.Name, cancellationToken);

        var response = new SaleDetailResponse(
            sale.Id,
            sale.TotalAmount,
            sale.PaymentMethod,
            sale.CustomerName,
            sale.User?.Name ?? "Usuário",
            sale.CreatedAt,
            sale.Items.Select(i => new SaleDetailItemResponse(
                i.ProductBarcode,
                productNames.GetValueOrDefault(i.ProductBarcode, "Produto"),
                i.Quantity,
                i.UnitPrice,
                i.UnitPrice * i.Quantity
            )).ToList()
        );

        return Result.Ok(response);
    }
}
