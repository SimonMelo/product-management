using Products.WebAPI.Common.Entities;

namespace Products.WebAPI.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}