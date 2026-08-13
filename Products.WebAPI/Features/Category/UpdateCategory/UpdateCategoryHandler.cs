using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Category.UpdateCategory;

public class UpdateCategoryHandler(AppDbContext db) : IRequestHandler<UpdateCategoryCommand, Result<UpdateCategoryResponse>>
{
    public async Task<Result<UpdateCategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await db.Categories.FindAsync([request.Id], cancellationToken);
        if (category is null)
            return Result.Fail<UpdateCategoryResponse>("Categoria não encontrada.");

        var duplicateName = await db.Categories.AnyAsync(c => c.Name == request.Name && c.Id != request.Id, cancellationToken);
        if (duplicateName)
            return Result.Fail<UpdateCategoryResponse>("Já existe outra categoria com esse nome.");

        category.Name = request.Name;
        category.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);

        return Result<UpdateCategoryResponse>.Ok(new UpdateCategoryResponse(category.Id, category.Name));
    }
}
