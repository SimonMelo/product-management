using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.WebAPI.Features.User.CreateUser;
using Products.WebAPI.Features.User.DeleteUser;
using Products.WebAPI.Features.User.GetUsers;
using Products.WebAPI.Features.User.UpdateUser;

namespace Products.WebAPI.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(ISender mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetUsersQuery(), cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }

    [HttpPatch("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request with { Id = id }, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteUserCommand(id), cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.ErrorMessage);
    }
}
