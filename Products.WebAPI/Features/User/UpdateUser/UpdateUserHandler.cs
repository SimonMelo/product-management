using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Interfaces;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.User.UpdateUser;

public class UpdateUserHandler(AppDbContext db, IPasswordHasher passwordHasher)
    : IRequestHandler<UpdateUserCommand, Result<UpdateUserResponse>>
{
    public async Task<Result<UpdateUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await db.Users.FindAsync([request.Id], cancellationToken);
        if (user is null)
            return Result.Fail<UpdateUserResponse>("Usuário não encontrado.");

        var duplicateEmail = await db.Users.AnyAsync(u => u.Email == request.Email && u.Id != request.Id, cancellationToken);
        if (duplicateEmail)
            return Result.Fail<UpdateUserResponse>("Já existe outro usuário com esse e-mail.");

        user.Name = request.Name;
        user.Email = request.Email;
        user.Role = request.Role;
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(request.Password))
            user.Password = passwordHasher.Hash(request.Password);

        await db.SaveChangesAsync(cancellationToken);

        return Result<UpdateUserResponse>.Ok(new UpdateUserResponse(user.Id, user.Name, user.Email, user.Role, user.IsActive));
    }
}
