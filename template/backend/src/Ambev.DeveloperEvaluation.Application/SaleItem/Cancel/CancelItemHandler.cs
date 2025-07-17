namespace Ambev.DeveloperEvaluation.Application.SaleItem.Cancel;

using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

public class CancelItemHandler : IRequestHandler<CancelItemCommand, Unit>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelItemHandler> _logger;
    private readonly IMediator _mediator;

    public CancelItemHandler(ISaleRepository saleRepository, ILogger<CancelItemHandler> logger, IMediator mediator)
    {
        _saleRepository = saleRepository;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(CancelItemCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException("Sale not found.");

        var item = sale.Items.FirstOrDefault(i => i.Id == command.ItemId);
        if (item == null)
            throw new KeyNotFoundException("Sale item not found.");

        // Mark item as cancelled (assuming a flag or status)
        item.Status = SaleStatus.Cancelled;

        // Optionally update sale totals here
        sale.TotalAmount = sale.Items.Where(i => i.Status != SaleStatus.Cancelled).Sum(i => i.Total);

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        _logger.LogInformation("Event: ItemCancelled - SaleNumber: {SaleNumber} - ItemId: {ItemId}", sale.SaleNumber, item.Id);

        await _mediator.Publish(new ItemCancelledEvent(sale.Id, item.Id), cancellationToken);

        return Unit.Value;
    }
}
