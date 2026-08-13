using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Interfaces;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Products.UploadProductImage;

public class UploadProductImageHandler(AppDbContext db, IFileStorageService fileStorageService)
    : IRequestHandler<UploadProductImageCommand, Result<UploadProductImageResponse>>
{
    public async Task<Result<UploadProductImageResponse>> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Barcode == request.Barcode, cancellationToken);

        if (product is null)
            return Result.Fail<UploadProductImageResponse>("Produto não encontrado.");

        if (!string.IsNullOrEmpty(product.VirtualPath))
        {
            try
            {
                await fileStorageService.DeleteAsync(product.VirtualPath, cancellationToken);
            }
            catch
            {
                // Ignora erro ao deletar imagem anterior
            }
        }

        await using var stream = request.File.OpenReadStream();
        var extension = Path.GetExtension(request.File.FileName);
        var objectName = $"product/{product.Brand!.Name}/{product.Category!.Name}/{product.Name}{extension}";

        var virtualPath = await fileStorageService.UploadFileAsync(
            objectName,
            stream,
            request.File.ContentType,
            cancellationToken);

        product.VirtualPath = virtualPath;
        product.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);

        var presignedUrl = await fileStorageService.GetPresignedUrlAsync(virtualPath, cancellationToken);

        return Result<UploadProductImageResponse>.Ok(new UploadProductImageResponse(product.Barcode, presignedUrl));
    }
}
