using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.WebAPI.Features.Category.CreateCategory;
using Products.WebAPI.Features.Category.DeleteCategory;
using Products.WebAPI.Features.Category.GetCategory;
using Products.WebAPI.Features.Category.UpdateCategory;

namespace Products.WebAPI.Controllers;

[ApiController]
[Route("api/category")]
    
public class CategoryController(ISender mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddCategory(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }

    [HttpGet]
    [Authorize(Policy = "CommonOrAdmin")]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetCategoriesQuery(), cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }

    [HttpPatch("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request with { Id = id }, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }
}
