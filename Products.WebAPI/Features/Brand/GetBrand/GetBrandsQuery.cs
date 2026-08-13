using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Brand.GetBrand;

public record GetBrandsQuery() : IRequest<Result<List<GetBrandsResponse>>>;

public record GetBrandsResponse(
    int Id,
    string Name,
    DateTime CreatedAt,
    DateTime? UpdatedAt);