using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Auth;

public record AuthCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;

public record AuthResponse(string Token);