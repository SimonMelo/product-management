using System.Security.Claims;
using Products.WebAPI.Common.Interfaces;

namespace Products.WebAPI.Infrastructure.Authentication;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public int Id =>
        int.Parse(User?.FindFirst("Id")?.Value ?? "0");

    public string Name =>
        User?.FindFirst("Name")?.Value ?? string.Empty;

    public string Email =>
        User?.FindFirst("Email")?.Value ?? string.Empty;

    public string Role =>
        User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
}