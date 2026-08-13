using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.WebAPI.Common.Entities;

namespace Products.WebAPI.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Name).IsRequired().HasMaxLength(30);
        builder.Property(m => m.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        builder.Property(m => m.UpdatedAt);
        
        builder.HasIndex(m => m.Name).IsUnique();
    }
}