using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Products.WebAPI.Common.Entities;
using Products.WebAPI.Common.Interfaces;

namespace Products.WebAPI.Infrastructure.Authentication;

public class JwtTokenGenerator(IConfiguration configuration)
    : IJwtTokenGenerator
{
    public string GenerateToken(User user)
    {
        var issuer = configuration["Jwt:Issuer"]
                     ?? throw new InvalidOperationException("Jwt:Issuer não configurado.");

        var audience = configuration["Jwt:Audience"]
                       ?? throw new InvalidOperationException("Audience não configurado.");

        var secret = configuration["Jwt:Key"]
                     ?? throw new InvalidOperationException("Secret não configurado.");

        var expiration =
            int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new("Name", user.Name),
            new("Email", user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secret));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}