using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.WebAPI.Common.Entities;

namespace Products.WebAPI.Infrastructure.Persistence.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("stock_movements");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Type).HasConversion<string>().HasMaxLength(20);
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        builder.HasIndex(m => new { m.ProductBarcode, m.CreatedAt, m.Type });
        builder.HasOne(m => m.Product)
            .WithMany(p => p.StockMovements)
            .HasForeignKey(m => m.ProductBarcode);
        builder.HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId);
        builder.HasOne(m => m.Sale)
            .WithMany()
            .HasForeignKey(m => m.SaleId);
    }
}