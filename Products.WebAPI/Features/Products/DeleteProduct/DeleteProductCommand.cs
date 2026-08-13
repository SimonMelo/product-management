using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Products.DeleteProduct;

public record DeleteProductCommand(string Barcode) : IRequest<Result<DeleteProductResponse>>;

public record DeleteProductResponse(bool Success);
