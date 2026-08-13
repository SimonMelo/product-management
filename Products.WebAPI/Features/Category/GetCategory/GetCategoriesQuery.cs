using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Category.GetCategory;

public record GetCategoriesQuery() : IRequest<Result<List<GetCategoriesResponse>>>;

public record GetCategoriesResponse(
    int Id,
    string Name,
    DateTime CreatedAt,
    DateTime? UpdatedAt
    );