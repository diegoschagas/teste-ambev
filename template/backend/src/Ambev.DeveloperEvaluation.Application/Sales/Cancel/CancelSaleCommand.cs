using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Cancel;
public class CancelSaleCommand : IRequest<Unit>
{
    public Guid SaleId { get; set; }
}
