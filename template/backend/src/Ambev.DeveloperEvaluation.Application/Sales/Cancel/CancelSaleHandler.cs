using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Cancel;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly IMediator _mediator;

    public CancelSaleHandler(ISaleRepository saleRepository, ILogger<CancelSaleHandler> logger, IMediator mediator)
    {
        _saleRepository = saleRepository;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Sale not found.");

        sale.Status = SaleStatus.Cancelled;

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        _logger.LogInformation("Event: SaleCancelled - SaleNumber: {SaleNumber}", sale.SaleNumber);

        await _mediator.Publish(new SaleCancelledEvent(sale.Id, sale.SaleNumber), cancellationToken);

        return new CancelSaleResult
        {
            SaleNumber = sale.SaleNumber,
            Status = sale.Status,
        };
    }
}
