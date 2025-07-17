using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Cancel;
public class CancelSaleCommand : IRequest<CancelSaleResult>
{
    public Guid Id { get; }

    public CancelSaleCommand(Guid id)
    {
        Id = id;
    }
}
