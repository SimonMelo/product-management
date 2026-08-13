using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Brand.UpdateBrand;

public class UpdateBrandHandler(AppDbContext db) : IRequestHandler<UpdateBrandCommand, Result<UpdateBrandResponse>>
{
    public async Task<Result<UpdateBrandResponse>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await db.Brands.FindAsync([request.Id], cancellationToken);
        if (brand is null)
            return Result.Fail<UpdateBrandResponse>("Marca não encontrada.");

        var duplicateName = await db.Brands.AnyAsync(b => b.Name == request.Name && b.Id != request.Id, cancellationToken);
        if (duplicateName)
            return Result.Fail<UpdateBrandResponse>("Já existe outra marca com esse nome.");

        brand.Name = request.Name;
        brand.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);

        return Result<UpdateBrandResponse>.Ok(new UpdateBrandResponse(brand.Id, brand.Name));
    }
}
