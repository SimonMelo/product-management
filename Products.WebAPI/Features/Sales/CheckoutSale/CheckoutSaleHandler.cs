using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Common.Entities;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Interfaces;
using Products.WebAPI.Common.Results;
using Products.WebAPI.Infrastructure.Persistence;

namespace Products.WebAPI.Features.Sales.CheckoutSale;

public class CheckoutSaleHandler(AppDbContext db, ICurrentUserService currentUserService) : IRequestHandler<CheckoutSaleCommand, Result<SaleResponse>>
{
    public async Task<Result<SaleResponse>> Handle(CheckoutSaleCommand request, CancellationToken cancellationToken)
    {
        var groupedItems = request.Items
            .GroupBy(i => i.Barcode)
            .Select(g => new CheckoutItemRequest(g.Key, g.Sum(i => i.Quantity)))
            .ToList();

        var barcodes = groupedItems.Select(i => i.Barcode).ToList();

        var products = await db.Products
            .Where(p => barcodes.Contains(p.Barcode))
            .ToDictionaryAsync(p => p.Barcode, cancellationToken);
        
        var missingBarcodes = barcodes.Where(b => !products.ContainsKey(b)).ToList();
        if (missingBarcodes.Count != 0)
            return Result.Fail<SaleResponse>($"Produto(s) não encontrado(s): {string.Join(", ", missingBarcodes)}");

        var sale = new Sale()
        {
            UserId = currentUserService.Id,
            PaymentMethod = request.PaymentMethod,
            CustomerName = request.CustomerName,
            CreatedAt = DateTime.UtcNow
        };

        decimal total = 0;

        foreach (var item in groupedItems)
        {
            var product = products[item.Barcode];
            var unitPrice = product.Price;
            total += unitPrice * item.Quantity;

            sale.Items.Add(new SaleItem()
            {
                ProductBarcode = item.Barcode,
                Quantity = item.Quantity,
                UnitPrice = unitPrice,
            });

            db.StockMovements.Add(new Common.Entities.StockMovement()
            {
                ProductBarcode = item.Barcode,
                Quantity = -item.Quantity,
                Type = EMovementType.VendaSaida,
                UserId = currentUserService.Id,
                Sale = sale,
                CreatedAt = DateTime.UtcNow
            });
        }

        sale.TotalAmount = total;
            
            db.Sales.Add(sale);
            await db.SaveChangesAsync(cancellationToken);

            var userName = await db.Users
                .Where(u => u.Id == currentUserService.Id)
                .Select(u => u.Name)
                .FirstOrDefaultAsync(cancellationToken) ?? "Usuário";

            var response = new SaleResponse(sale.Id, sale.TotalAmount, sale.PaymentMethod, sale.CustomerName, userName, sale.CreatedAt,
                sale.Items.Select(i => new SaleItemResponse(i.ProductBarcode,
                    products[i.ProductBarcode].Name,
                    i.Quantity,
                    i.UnitPrice,
                    i.UnitPrice * i.Quantity)).ToList());
            
            return Result<SaleResponse>.Ok(response);
        }
}