using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Products.GetProducts;

public record GetProductQuery(string? Name, int? CategoryId, int? BrandId, string? Status) : IRequest<Result<List<GetProductResponse>>>;