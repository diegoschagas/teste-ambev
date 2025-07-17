using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.SearchSale;

public class ListSaleResult
{
    public Guid? Id { get; set; }
    public string? SaleNumber { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string? Customer { get; set; } = string.Empty;
    public decimal? TotalAmount { get; set; }
    public string? Branch { get; set; } = string.Empty;
    public SaleStatus Status { get; set; } = SaleStatus.Active; 
    public List<CreateSaleItemDto>? Items { get; set; }
}
