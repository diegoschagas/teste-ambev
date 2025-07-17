
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Modify;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Modify;

public class ModifySaleCommand : IRequest<ModifySaleResult>
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Customer { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Branch { get; set; } = string.Empty;
    public SaleStatus Status { get; set; } = SaleStatus.Active; 
    public List<CreateSaleItemDto>? Items { get; set; }
}
