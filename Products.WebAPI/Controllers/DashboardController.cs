using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.WebAPI.Features.Dashboard.GetDashboardSummary;

namespace Products.WebAPI.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController(ISender mediator) : ControllerBase
{
    [HttpGet("summary")]
    [Authorize(Policy = "CommonOrAdmin")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetDashboardSummaryQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }
}
