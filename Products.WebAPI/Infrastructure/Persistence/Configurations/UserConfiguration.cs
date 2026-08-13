using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.WebAPI.Common.Entities;

namespace Products.WebAPI.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Password).IsRequired().HasMaxLength(256);
        builder.Property(p => p.Role).IsRequired().HasConversion<string>().HasMaxLength(10);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(100);
        builder.Property(p => p.IsActive).HasDefaultValue(true);
        builder.Property(p => p.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        builder.Property(p => p.UpdatedAt);
        
        builder.HasIndex(p => p.Email).IsUnique();
    }
}