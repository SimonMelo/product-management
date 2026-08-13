using MediatR;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Sales.GetSaleById;

public record GetSaleByIdQuery(int Id) : IRequest<Result<SaleDetailResponse>>;

public record SaleDetailResponse(
    int Id,
    decimal TotalAmount,
    EPaymentMethod PaymentMethod,
    string? CustomerName,
    string UserName,
    DateTime CreatedAt,
    List<SaleDetailItemResponse> Items
);

public record SaleDetailItemResponse(
    string Barcode,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal
);
