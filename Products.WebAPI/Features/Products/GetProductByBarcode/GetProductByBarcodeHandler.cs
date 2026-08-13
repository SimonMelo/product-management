using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Products.GetProductByBarcode;

public class GetProductByBarcodeHandler(AppDbContext db, IConfiguration config)
    : IRequestHandler<GetProductByBarcodeQuery, Result<GetProductByBarcodeResponse>>
{
    public async Task<Result<GetProductByBarcodeResponse>> Handle(GetProductByBarcodeQuery request,
        CancellationToken cancellationToken)
    {
        var product = await db.Products
            .AsNoTracking()
            .Where(p => p.Barcode == request.Barcode)
            .Select(p => new
            {
                p.Barcode,
                p.Name,
                p.CategoryId,
                Category = p.Category!.Name,
                p.BrandId,
                Brand = p.Brand!.Name,
                p.Price,
                p.Disp,
                p.VirtualPath,
                Stock = db.StockMovements
                    .Where(m => m.ProductBarcode == p.Barcode)
                    .Sum(m => (int?)m.Quantity) ?? 0
            }).FirstOrDefaultAsync(cancellationToken);

        if (product is null)
            return Result.Fail<GetProductByBarcodeResponse>("Produto não encontrado no sistema.");

        var status = product.Stock <= 0
            ? "esgotado"
            : (product.Disp ? "disponivel" : "em_estoque");

        var minioEndpoint = config["Minio:Endpoint"] ?? "localhost:9000";
        string? imageUrl = null;
        if (!string.IsNullOrEmpty(product.VirtualPath))
            imageUrl = $"http://{minioEndpoint}/products/{product.VirtualPath}";

        var brands = await db.Brands
            .AsNoTracking()
            .Select(b => new BrandOptionResponse(b.Id, b.Name))
            .ToListAsync(cancellationToken);

        var categories = await db.Categories
            .AsNoTracking()
            .Select(c => new CategoryOptionResponse(c.Id, c.Name))
            .ToListAsync(cancellationToken);

        var movements = await db.StockMovements
            .AsNoTracking()
            .Where(m => m.ProductBarcode == request.Barcode)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new ProductMovementResponse(
                m.Id,
                m.ProductBarcode,
                product.Name,
                m.Quantity,
                m.Type.ToString(),
                m.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        return Result<GetProductByBarcodeResponse>.Ok(new GetProductByBarcodeResponse(
            product.Barcode,
            product.Name,
            product.CategoryId,
            product.Category,
            product.BrandId,
            product.Brand,
            product.Price,
            product.Disp,
            product.Stock,
            status,
            imageUrl,
            brands,
            categories,
            movements));
    }
}
