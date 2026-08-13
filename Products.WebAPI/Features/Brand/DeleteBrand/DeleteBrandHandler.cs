using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Brand.DeleteBrand;

public class DeleteBrandHandler(AppDbContext db) : IRequestHandler<DeleteBrandCommand, Result<DeleteBrandResponse>>
{
    public async Task<Result<DeleteBrandResponse>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await db.Brands.FindAsync([request.Id], cancellationToken);
        if (brand is null)
            return Result.Fail<DeleteBrandResponse>("Marca não encontrada.");

        var hasProducts = await db.Products.AnyAsync(p => p.BrandId == request.Id, cancellationToken);
        if (hasProducts)
            return Result.Fail<DeleteBrandResponse>("Não é possível remover uma marca que possui produtos vinculados.");

        db.Brands.Remove(brand);
        await db.SaveChangesAsync(cancellationToken);

        return Result<DeleteBrandResponse>.Ok(new DeleteBrandResponse(true));
    }
}
