using MediatR;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Sales.CheckoutSale;

public record CheckoutSaleCommand(List<CheckoutItemRequest> Items, EPaymentMethod PaymentMethod, int UserId, string? CustomerName) : IRequest<Result<SaleResponse>>;

public record SaleResponse(int Id, decimal TotalAmount, EPaymentMethod PaymentMethod, string? CustomerName, string UserName, DateTime CreatedAt, List<SaleItemResponse> Items);

public record SaleItemResponse(string Barcode, string ProductName, int Quantity, decimal UnitPrice, decimal Subtotal);