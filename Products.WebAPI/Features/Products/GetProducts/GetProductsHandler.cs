using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Products.GetProducts;

public class GetProductsHandler(AppDbContext db, IConfiguration config)
    : IRequestHandler<GetProductQuery, Result<List<GetProductResponse>>>
{
    public async Task<Result<List<GetProductResponse>>> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var productsQuery = db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
            productsQuery = productsQuery.Where(p => p.Name.Contains(query.Name));

        if (query.CategoryId.HasValue)
            productsQuery = productsQuery.Where(p => p.CategoryId == query.CategoryId);

        if (query.BrandId.HasValue)
            productsQuery = productsQuery.Where(p => p.BrandId == query.BrandId);

        var projectedQuery = productsQuery
            .Select(p => new
            {
                p.Barcode,
                p.Name,
                Category = p.Category!.Name,
                Brand = p.Brand!.Name,
                p.VirtualPath,
                p.Disp,
                p.Price,
                Stock = db.StockMovements
                    .Where(m => m.ProductBarcode == p.Barcode)
                    .Sum(m => (int?)m.Quantity) ?? 0
            })
            .Select(p => new
            {
                p.Barcode,
                p.Name,
                p.Category,
                p.Brand,
                p.VirtualPath,
                p.Disp,
                p.Price,
                p.Stock,
                Status = p.Stock <= 0
                    ? "esgotado"
                    : (p.Disp ? "disponivel" : "em_estoque")
            });

        if (!string.IsNullOrWhiteSpace(query.Status))
            projectedQuery = projectedQuery.Where(p => p.Status == query.Status);

        var result = await projectedQuery.ToListAsync(cancellationToken);

        var minioEndpoint = config["Minio:Endpoint"] ?? "localhost:9000";

        var dtos = result.Select(p => new GetProductResponse(
            p.Barcode,
            p.Name,
            p.Category,
            p.Brand,
            string.IsNullOrEmpty(p.VirtualPath) ? null : $"http://{minioEndpoint}/products/{p.VirtualPath}",
            p.Disp,
            p.Price,
            p.Stock,
            p.Status
        )).ToList();

        return Result.Ok(dtos);
    }
}
