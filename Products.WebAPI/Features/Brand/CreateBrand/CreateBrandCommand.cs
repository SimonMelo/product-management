using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Brand.CreateBrand;

public record CreateBrandCommand(string Name) : IRequest<Result<CreateBrandResponse>>;

public record CreateBrandResponse(int Id, string Name);