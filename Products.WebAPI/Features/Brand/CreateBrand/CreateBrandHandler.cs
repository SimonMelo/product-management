using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Brand.CreateBrand;

public class CreateBrandHandler(AppDbContext db) : IRequestHandler<CreateBrandCommand, Result<CreateBrandResponse>>
{
    public async Task<Result<CreateBrandResponse>> Handle(CreateBrandCommand request,
        CancellationToken cancellationToken)
    {
        var brandExist = await db.Brands.AnyAsync(b => b.Name == request.Name, cancellationToken);
        if (brandExist)
            return Result.Fail<CreateBrandResponse>("Marca ja cadastrada no sistema");

        var brand = new Common.Entities.Brand()
        {
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        db.Add(brand);

        await db.SaveChangesAsync(cancellationToken);
        
        return Result<CreateBrandResponse>.Ok(new CreateBrandResponse(brand.Id, brand.Name));
    }
}