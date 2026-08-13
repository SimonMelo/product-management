using MediatR;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Sales.GetSales;

public record GetSalesQuery() : IRequest<Result<List<SaleListItemResponse>>>;

public record SaleListItemResponse(
    int Id,
    decimal TotalAmount,
    EPaymentMethod PaymentMethod,
    string? CustomerName,
    string UserName,
    int ItemCount,
    DateTime CreatedAt
);
