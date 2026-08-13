using Products.WebAPI.Common.Enums;

namespace Products.WebAPI.Features.Sales.CheckoutSale;

public record CheckoutSaleRequest(List<CheckoutItemRequest> Items, EPaymentMethod PaymentMethod, string? CustomerName);

public record CheckoutItemRequest(string Barcode, int Quantity);