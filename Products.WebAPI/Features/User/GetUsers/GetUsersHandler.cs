using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.User.GetUsers;

public class GetUsersHandler(AppDbContext db)
    : IRequestHandler<GetUsersQuery, Result<List<GetUsersResponse>>>
{
    public async Task<Result<List<GetUsersResponse>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await db.Users
            .AsNoTracking()
            .Select(u => new GetUsersResponse(
                u.Id,
                u.Name,
                u.Email,
                u.Role,
                u.IsActive
            ))
            .ToListAsync(cancellationToken);

        return Result.Ok(users);
    }
}
