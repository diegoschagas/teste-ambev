namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.SearchSale;

public class SearchSalesRequest
{
    public string? SaleNumber { get; set; }
    public DateTime? Date { get; set; }
    public string? Customer { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? Branch { get; set; }
}
