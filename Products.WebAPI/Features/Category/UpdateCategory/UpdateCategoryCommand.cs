using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Category.UpdateCategory;

public record UpdateCategoryCommand(int Id, string Name) : IRequest<Result<UpdateCategoryResponse>>;

public record UpdateCategoryResponse(int Id, string Name);
