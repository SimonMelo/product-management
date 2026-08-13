using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.WebAPI.Common.Interfaces;
using Products.WebAPI.Features.Sales.CheckoutSale;
using Products.WebAPI.Features.Sales.GetSaleById;
using Products.WebAPI.Features.Sales.GetSales;

namespace Products.WebAPI.Controllers;

[ApiController]
[Route("api/sales")]
public class SalesController(ISender mediator, ICurrentUserService currentUserService) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "CommonOrAdmin")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetSalesQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "CommonOrAdmin")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSaleByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpPost("checkout")]
    [Authorize(Policy = "CommonOrAdmin")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutSaleRequest request, CancellationToken ct)
    {
        var command = new CheckoutSaleCommand(request.Items, request.PaymentMethod, currentUserService.Id, request.CustomerName);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }
}