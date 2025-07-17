namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleResult
{
    public Guid? Id { get; set; }
    public string? SaleNumber { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string? Customer { get; set; } = string.Empty;
    public string? Branch { get; set; } = string.Empty;
    public List<CreateSaleItemDto>? Items { get; set; }
}
