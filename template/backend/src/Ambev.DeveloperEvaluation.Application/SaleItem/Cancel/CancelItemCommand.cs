using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItem.Cancel;

public class CancelItemCommand : IRequest<Unit>
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
}
