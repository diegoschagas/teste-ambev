using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Modify;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Modify;


public class ModifySaleHandler : IRequestHandler<ModifySaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ModifySaleHandler> _logger;
    private readonly IMediator _mediator;

    public ModifySaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<ModifySaleHandler> logger, IMediator mediator)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<CreateSaleResult> Handle(ModifySaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException("Sale not found.");

        sale.Items = _mapper.Map<List<Ambev.DeveloperEvaluation.Domain.Entities.SaleItem>>(command.Items);

        // Recalculate totals
        foreach (var item in sale.Items)
        {
            item.Total = (item.Quantity * item.UnitPrice) - item.Discount;
        }
        sale.TotalAmount = sale.Items.Sum(i => i.Total);

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        _logger.LogInformation("Event: SaleModified - SaleNumber: {SaleNumber}", sale.SaleNumber);

        await _mediator.Publish(new SaleModifiedEvent(sale.Id, sale.SaleNumber), cancellationToken);

        return _mapper.Map<CreateSaleResult>(sale);
    }
}