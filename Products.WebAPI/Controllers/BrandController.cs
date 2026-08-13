using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.WebAPI.Features.Brand.CreateBrand;
using Products.WebAPI.Features.Brand.DeleteBrand;
using Products.WebAPI.Features.Brand.GetBrand;
using Products.WebAPI.Features.Brand.UpdateBrand;

namespace Products.WebAPI.Controllers;

[ApiController]
[Route("api/brand")]
public class BrandController(ISender mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddBrand(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }
    
    [HttpGet]
    [Authorize(Policy = "CommonOrAdmin")]
    public async Task<IActionResult> GetBrands(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetBrandsQuery(), cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }

    [HttpPatch("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateBrand(int id, UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request with { Id = id }, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteBrand(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteBrandCommand(id), cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }
}
