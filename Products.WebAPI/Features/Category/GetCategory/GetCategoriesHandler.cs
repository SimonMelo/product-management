using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Features.Category.GetCategory;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Categories.GetCategories;

public class GetCategoriesHandler(AppDbContext db)
    : IRequestHandler<GetCategoriesQuery, Result<List<GetCategoriesResponse>>>
{
    public async Task<Result<List<GetCategoriesResponse>>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await db.Categories
            .AsNoTracking()
            .Select(c => new GetCategoriesResponse(
                c.Id,
                c.Name,
                c.CreatedAt,
                c.UpdatedAt
            ))
            .ToListAsync(cancellationToken);

        return Result.Ok(categories);
    }
}