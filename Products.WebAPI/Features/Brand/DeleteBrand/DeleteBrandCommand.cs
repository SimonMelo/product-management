using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Brand.DeleteBrand;

public record DeleteBrandCommand(int Id) : IRequest<Result<DeleteBrandResponse>>;

public record DeleteBrandResponse(bool Success);
