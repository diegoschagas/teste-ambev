using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItem.CreateSaleItem;

public class CreateSaleItemCommand : IRequest<CreateSaleItemResult>
{
    public string? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
