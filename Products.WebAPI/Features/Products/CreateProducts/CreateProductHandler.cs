using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Interfaces;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Products.CreateProducts;

public class CreateProductHandler(AppDbContext db, ICurrentUserService currentUserService) : IRequestHandler<CreateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var barcodeExists = await db.Products.AnyAsync(p => p.Barcode == request.Barcode, cancellationToken);
        if (barcodeExists)
            return Result.Fail<ProductResponse>("Já existe um produto com esse código de barras cadastrado.");

        var duplicateNameBrand =
            await db.Products.AnyAsync(p => p.Name == request.Name && p.BrandId == request.BrandId, cancellationToken);
        if (duplicateNameBrand)
            return Result.Fail<ProductResponse>("Já existe um produto com esse nome para essa marca cadastrado.");

        var brand = db.Brands.FirstOrDefault(b => b.Id == request.BrandId);
        if (brand is null)
            return Result.Fail<ProductResponse>("Marca não encontrada.");
        
        var category = db.Categories.FirstOrDefault(b => b.Id == request.CategoryId);
        if (category is null)
            return Result.Fail<ProductResponse>("Categoria não encontrada.");
        
        var product = new Common.Entities.Products()
        {
            Barcode = request.Barcode,
            Name = request.Name,
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,
            Disp = request.Disp,
            Price = request.Price,
            VirtualPath = string.Empty,
            UserId = currentUserService.Id,
            CreatedAt = DateTime.UtcNow
        };
        
        var movement = new Common.Entities.StockMovement
        {
            ProductBarcode = request.Barcode,
            Quantity = request.Quantity,
            Type = EMovementType.Ajuste,
            UserId = currentUserService.Id,
            CreatedAt = DateTime.UtcNow
        };

        db.Products.Add(product);
        
        await db.SaveChangesAsync(cancellationToken);
        
        db.StockMovements.Add(movement);

        await db.SaveChangesAsync(cancellationToken);
        
        return Result<ProductResponse>.Ok(new ProductResponse(request.Barcode, request.Name, brand.Name, category.Name, request.Quantity));
    }
    
}