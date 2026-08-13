using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.User.DeleteUser;

public record DeleteUserCommand(int Id) : IRequest<Result<DeleteUserResponse>>;

public record DeleteUserResponse(bool Success);
