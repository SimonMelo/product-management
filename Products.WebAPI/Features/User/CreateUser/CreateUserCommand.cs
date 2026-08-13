using MediatR;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.User.CreateUser;

public record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    ERoles Role
) : IRequest<Result<CreateUserResponse>>;

public record CreateUserResponse(
    int Id,
    string Name,
    string Email,
    ERoles Role
);