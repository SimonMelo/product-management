using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Products.UpdateProduct;

public class UpdateProductHandler(AppDbContext db) : IRequestHandler<UpdateProductCommand, Result<UpdateProductResponse>>
{
    public async Task<Result<UpdateProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await db.Products.FindAsync([request.Barcode], cancellationToken);
        if (product is null)
            return Result.Fail<UpdateProductResponse>("Produto não encontrado.");

        var brand = await db.Brands.FindAsync([request.BrandId], cancellationToken);
        if (brand is null)
            return Result.Fail<UpdateProductResponse>("Marca não encontrada.");

        var category = await db.Categories.FindAsync([request.CategoryId], cancellationToken);
        if (category is null)
            return Result.Fail<UpdateProductResponse>("Categoria não encontrada.");

        var duplicateNameBrand = await db.Products.AnyAsync(
            p => p.Name == request.Name && p.BrandId == request.BrandId && p.Barcode != request.Barcode,
            cancellationToken);
        if (duplicateNameBrand)
            return Result.Fail<UpdateProductResponse>("Já existe um produto com esse nome para essa marca.");

        product.Name = request.Name;
        product.CategoryId = request.CategoryId;
        product.BrandId = request.BrandId;
        product.Disp = request.Disp;
        product.Price = request.Price;
        product.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);

        return Result<UpdateProductResponse>.Ok(new UpdateProductResponse(product.Barcode, product.Name, brand.Name, category.Name));
    }
}
