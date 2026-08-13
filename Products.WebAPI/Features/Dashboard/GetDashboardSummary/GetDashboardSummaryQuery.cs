using MediatR;
using Products.WebAPI.Common.Results;

namespace Products.WebAPI.Features.Dashboard.GetDashboardSummary;

public record GetDashboardSummaryQuery() : IRequest<Result<DashboardSummaryResponse>>;

public record DashboardSummaryResponse(
    int TotalProdutos,
    int Disponiveis,
    int EmEstoque,
    int Esgotados
);
