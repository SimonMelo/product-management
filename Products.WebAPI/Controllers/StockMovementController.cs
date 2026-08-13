using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.WebAPI.Common.Interfaces;
using Products.WebAPI.Features.StockMovement.GetStockMovements;
using Products.WebAPI.Features.StockMovement.RegisterAdjustment;
using Products.WebAPI.Features.StockMovement.RegisterStockIn;

namespace Products.WebAPI.Controllers;

[ApiController]
[Route("api/stock-movement")]
public class StockMovementController(ISender mediator, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "CommonOrAdmin")]
    public async Task<IActionResult> GetStockMovements(
        [FromQuery] string? barcode,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetStockMovementsQuery(barcode), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpPost("in")]
    [Authorize(Policy = "CommonOrAdmin")]
    public async Task<IActionResult> RegisterStockIn([FromBody] RegisterStockInRequest request, CancellationToken ct)
    {
        var command = new RegisterStockInCommand(request.Items, currentUser.Id);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpPost("adjustment")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> RegisterAdjustment([FromBody] RegisterAdjustmentRequest request, CancellationToken ct)
    {
        var command = new RegisterAdjustmentCommand(request.Barcode, request.Quantity, currentUser.Id);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }
}
