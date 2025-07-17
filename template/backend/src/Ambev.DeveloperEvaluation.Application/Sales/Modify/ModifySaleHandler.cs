using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Modify;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Modify;


public class ModifySaleHandler : IRequestHandler<ModifySaleCommand, ModifySaleResult>
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

    public async Task<ModifySaleResult> Handle(ModifySaleCommand command, CancellationToken cancellationToken)
    {
        // Validate command
        var validationResult = await new ModifySaleCommandValidator().ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (command.Items != null && command.Items.Any(i => i.Quantity > 20))
            throw new InvalidOperationException("Cannot sell more than 20 identical items");

        // Retrieve existing sale
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with Id {command.Id} not found.");

        // Map the updates from command to existing sale entity
        // Here, careful to only update allowed properties, not IDs or system fields
        _mapper.Map(command, sale);

        // Recalculate total per item and total amount
        foreach (var item in sale.Items)
        {
            item.Total = (item.Quantity * item.UnitPrice) - item.Discount;
        }

        sale.TotalAmount = sale.Items.Sum(i => i.Total);

        // Update the sale in repository
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Map to result DTO
        var result = _mapper.Map<ModifySaleResult>(sale);

        _logger.LogInformation("Event: SaleUpdated - SaleNumber: {SaleNumber}", sale.SaleNumber);

        // Publish event for sale update
        await _mediator.Publish(new SaleModifiedEvent(sale.Id, sale.SaleNumber), cancellationToken);

        return result;
    }
}