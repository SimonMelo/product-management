using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Brand.UpdateBrand;

public record UpdateBrandCommand(int Id, string Name) : IRequest<Result<UpdateBrandResponse>>;

public record UpdateBrandResponse(int Id, string Name);
