using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Interfaces;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Products.DeleteProduct;

public class DeleteProductHandler(AppDbContext db, IFileStorageService fileStorageService)
    : IRequestHandler<DeleteProductCommand, Result<DeleteProductResponse>>
{
    public async Task<Result<DeleteProductResponse>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await db.Products.FindAsync([request.Barcode], cancellationToken);
        if (product is null)
            return Result.Fail<DeleteProductResponse>("Produto não encontrado.");

        if (!string.IsNullOrEmpty(product.VirtualPath))
        {
            await fileStorageService.DeleteAsync(product.VirtualPath, cancellationToken);
        }

        var movements = await db.StockMovements
            .Where(m => m.ProductBarcode == request.Barcode)
            .ToListAsync(cancellationToken);
        db.StockMovements.RemoveRange(movements);

        db.Products.Remove(product);
        await db.SaveChangesAsync(cancellationToken);

        return Result<DeleteProductResponse>.Ok(new DeleteProductResponse(true));
    }
}
