using MediatR;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.User.DeleteUser;

public class DeleteUserHandler(AppDbContext db) : IRequestHandler<DeleteUserCommand, Result<DeleteUserResponse>>
{
    public async Task<Result<DeleteUserResponse>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await db.Users.FindAsync([request.Id], cancellationToken);
        if (user is null)
            return Result.Fail<DeleteUserResponse>("Usuário não encontrado.");

        db.Users.Remove(user);
        await db.SaveChangesAsync(cancellationToken);

        return Result<DeleteUserResponse>.Ok(new DeleteUserResponse(true));
    }
}
