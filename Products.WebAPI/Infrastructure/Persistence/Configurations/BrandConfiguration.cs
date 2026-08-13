using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.WebAPI.Common.Entities;

namespace Products.WebAPI.Infrastructure.Persistence.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("brands");
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Name).IsRequired().HasMaxLength(128);
        builder.Property(m => m.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        builder.Property(m => m.UpdatedAt);
        
        builder.HasIndex(m => m.Name).IsUnique();
    }
}