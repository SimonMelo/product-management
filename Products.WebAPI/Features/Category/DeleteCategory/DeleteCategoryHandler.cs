using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Category.DeleteCategory;

public class DeleteCategoryHandler(AppDbContext db) : IRequestHandler<DeleteCategoryCommand, Result<DeleteCategoryResponse>>
{
    public async Task<Result<DeleteCategoryResponse>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await db.Categories.FindAsync([request.Id], cancellationToken);
        if (category is null)
            return Result.Fail<DeleteCategoryResponse>("Categoria não encontrada.");

        var hasProducts = await db.Products.AnyAsync(p => p.CategoryId == request.Id, cancellationToken);
        if (hasProducts)
            return Result.Fail<DeleteCategoryResponse>("Não é possível remover uma categoria que possui produtos vinculados.");

        db.Categories.Remove(category);
        await db.SaveChangesAsync(cancellationToken);

        return Result<DeleteCategoryResponse>.Ok(new DeleteCategoryResponse(true));
    }
}
