using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Category.CreateCategory;

public class CreateCategoryHandler(AppDbContext db) : IRequestHandler<CreateCategoryCommand, Result<CreateCategoryResponse>>
{
    public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryExist = await db.Categories.AnyAsync(b => b.Name == request.Name, cancellationToken);
        if (categoryExist)
            return Result.Fail<CreateCategoryResponse>("Categoria de produto ja cadastrada no sistema");

        var category = new Common.Entities.Category()
        {
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        db.Add(category);

        await db.SaveChangesAsync(cancellationToken);
        
        return Result<CreateCategoryResponse>.Ok(new CreateCategoryResponse(category.Id, category.Name));
    }
}