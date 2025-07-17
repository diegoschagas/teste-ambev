using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.SaleItem.CancelSaleItem;

public class CancelSaleItemResult
{
    public Guid SaleId { get; set; }
    public decimal TotalAmount { get; set; }
    public Guid ItemId { get; set; }
    public string Product { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string StatusDescription { get; set; } = string.Empty;
}
