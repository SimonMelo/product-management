using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Brand.GetBrand;

public class GetBrandsHandler(AppDbContext db)
    : IRequestHandler<GetBrandsQuery, Result<List<GetBrandsResponse>>>
{
    public async Task<Result<List<GetBrandsResponse>>> Handle(GetBrandsQuery query, CancellationToken cancellationToken)
    {
        var brands = await db.Brands
            .AsNoTracking()
            .Select(b => new GetBrandsResponse(
                b.Id,
                b.Name,
                b.CreatedAt,
                b.UpdatedAt
            ))
            .ToListAsync(cancellationToken);

        return Result.Ok(brands);
    }
}