using MediatR;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.User.GetUsers;

public record GetUsersQuery() : IRequest<Result<List<GetUsersResponse>>>;

public record GetUsersResponse(
    int Id,
    string Name,
    string Email,
    ERoles Role,
    bool IsActive
);
