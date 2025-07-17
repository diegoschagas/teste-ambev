using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.CancelSaleItem;

public class CancelSaleItemResponse
{
    public Guid SaleId { get; set; }
    public decimal TotalAmount { get; set; }
    public Guid ItemId { get; set; }
    public string Product { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string StatusDescription { get; set; } = string.Empty;
}
