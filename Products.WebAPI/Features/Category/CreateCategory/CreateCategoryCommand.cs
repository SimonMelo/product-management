using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Category.CreateCategory;

public record CreateCategoryCommand(string Name) : IRequest<Result<CreateCategoryResponse>>;

public record CreateCategoryResponse(int Id, string Name);