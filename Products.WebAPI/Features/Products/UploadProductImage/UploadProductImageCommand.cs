using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Products.UploadProductImage;

public record UploadProductImageCommand(string Barcode, IFormFile File) : IRequest<Result<UploadProductImageResponse>>;

public record UploadProductImageResponse(string Barcode, string VirtualPath);
