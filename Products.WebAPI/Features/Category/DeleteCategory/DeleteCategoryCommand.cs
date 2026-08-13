using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Category.DeleteCategory;

public record DeleteCategoryCommand(int Id) : IRequest<Result<DeleteCategoryResponse>>;

public record DeleteCategoryResponse(bool Success);
