using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Interfaces;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.User.CreateUser;

public class CreateUserHandler(AppDbContext db, IPasswordHasher passwordHasher)
    : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await db.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (emailExists)
        {
            return Result<CreateUserResponse>.Fail("Já existe um usuário cadastrado com este e-mail.");
        }

        var user = new Common.Entities.User()
        {
            Name = request.Name,
            Email = request.Email,
            Password = passwordHasher.Hash(request.Password),
            Role = request.Role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);

        return Result<CreateUserResponse>.Ok(new CreateUserResponse(user.Id, user.Name, user.Email, user.Role));
    }
}