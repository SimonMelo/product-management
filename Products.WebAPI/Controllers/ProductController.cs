using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.WebAPI.Features.Products.CreateProducts;
using Products.WebAPI.Features.Products.DeleteProduct;
using Products.WebAPI.Features.Products.GetProductByBarcode;
using Products.WebAPI.Features.Products.GetProducts;
using Products.WebAPI.Features.Products.UpdateProduct;
using Products.WebAPI.Features.Products.UploadProductImage;

namespace Products.WebAPI.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController(ISender mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "CommonOrAdmin")]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? name,
        [FromQuery] int? categoryId,
        [FromQuery] int? brandId,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductQuery(name, categoryId, brandId, status), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }
    
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.ErrorMessage);
    }
    
    [HttpGet("{barcode}")]
    [Authorize(Policy = "CommonOrAdmin")]
    public async Task<IActionResult> GetByBarcode(string barcode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductByBarcodeQuery(barcode), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.ErrorMessage);
    }

    [HttpPatch("{barcode}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateProduct(string barcode, UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request with { Barcode = barcode }, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{barcode}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteProduct(string barcode, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteProductCommand(barcode), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpPost("{barcode}/image")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UploadImage(string barcode, IFormFile file, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UploadProductImageCommand(barcode, file), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }
}
