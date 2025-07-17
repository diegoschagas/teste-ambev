using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.SearchSale;

public record SearchSaleCommand : IRequest<List<Sale>>
{
    public string? SaleNumber { get; set; }
    public DateTime? Date { get; set; }
    public string? Customer { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? Branch { get; set; }
}
