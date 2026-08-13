using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Products.WebAPI.Infrastructure.Persistence.Configurations;

public class ProductsConfiguration : IEntityTypeConfiguration<Common.Entities.Products>
{
    public void Configure(EntityTypeBuilder<Common.Entities.Products> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Barcode);
        builder.Property(p => p.Barcode).HasMaxLength(50);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.VirtualPath).HasMaxLength(200);
        builder.Property(p => p.Disp).HasDefaultValue(true);
        builder.Property(p => p.Price).HasPrecision(10, 2).HasDefaultValue(10.00m);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        builder.Property(p => p.UpdatedAt);
        builder.HasIndex(p => new { p.Name, p.BrandId }).IsUnique();
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId);
        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId);
    }
}