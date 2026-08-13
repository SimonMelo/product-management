using MediatR;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.User.UpdateUser;

public record UpdateUserCommand(
    int Id,
    string Name,
    string Email,
    ERoles Role,
    bool IsActive,
    string? Password
) : IRequest<Result<UpdateUserResponse>>;

public record UpdateUserResponse(
    int Id,
    string Name,
    string Email,
    ERoles Role,
    bool IsActive
);
