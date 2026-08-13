using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Interfaces;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Auth;

public class AuthHandler(AppDbContext db, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<AuthCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(AuthCommand request, CancellationToken cancellationToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(p => p.Email == request.Email, cancellationToken);
        if (user is null)
            return Result.Fail<AuthResponse>("Usuário não cadastrado ou não encontrado");

        var verifyPassword = passwordHasher.Verify(request.Password, user.Password);
        if (!verifyPassword)
            return Result.Fail<AuthResponse>("Senha incorreta");

        var token = jwtTokenGenerator.GenerateToken(user);

        return Result<AuthResponse>.Ok(new AuthResponse(token));
    }
}