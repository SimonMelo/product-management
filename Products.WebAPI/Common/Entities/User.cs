using Products.WebAPI.Common.Enums;

namespace Products.WebAPI.Common.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ERoles Role { get; set; }
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}