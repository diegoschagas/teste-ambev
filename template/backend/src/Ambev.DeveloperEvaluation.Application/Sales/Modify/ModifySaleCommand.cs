
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Modify;

public class ModifySaleCommand : IRequest<CreateSaleResult>
{
    public Guid SaleId { get; set; }
    
    public List<CreateSaleItemDto>? Items { get; set; }
}
