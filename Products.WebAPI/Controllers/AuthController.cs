using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.WebAPI.Features.Auth;

namespace Products.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController(ISender mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] AuthCommand request, CancellationToken ct)
    {
        var response = await mediator.Send(request, ct);
        return response.IsSuccess
            ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }
}