namespace Products.WebAPI.Common.Interfaces;

public interface ICurrentUserService
{
    int Id { get; }
    string Name { get; }
    string Email { get; }
    string Role { get; }
    
    bool IsAuthenticated { get; }
}