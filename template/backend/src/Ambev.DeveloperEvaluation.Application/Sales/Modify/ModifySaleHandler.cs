using Ambev.DeveloperEvaluation.Application.Sales.Modify;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Modify;
using AutoMapper;
using FluentValidation;
// No other changes are needed in the file.
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ModifySaleHandler : IRequestHandler<ModifySaleCommand, ModifySaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ModifySaleHandler> _logger;
    private readonly IMediator _mediator;

    public ModifySaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<ModifySaleHandler> logger, IMediator mediator, ISaleItemRepository saleItemRepository)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
        _saleItemRepository = saleItemRepository;
    }

    public async Task<ModifySaleResult> Handle(ModifySaleCommand command, CancellationToken cancellationToken)
    {
        // Validate command
        var validator = new ModifySaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Check quantity limits
        if (command.Items != null && command.Items.Any(i => i.Quantity > 20))
            throw new InvalidOperationException("Cannot sell more than 20 identical items");

        // Retrieve existing sale
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with Id {command.Id} not found.");

        // Update sale properties from command
        sale.SaleNumber = command.SaleNumber ?? sale.SaleNumber;
        sale.Date = command.Date ?? sale.Date;
        sale.Customer = command.Customer ?? sale.Customer;
        sale.TotalAmount = command.TotalAmount ?? sale.TotalAmount;
        sale.Branch = command.Branch ?? sale.Branch;
        sale.Status = command.Status ?? sale.Status;

        // Update items
        if (command.Items != null)
        {
            sale.Items.Clear(); // Assuming EF tracking or domain design supports direct list replacement

            foreach (var itemCommand in command.Items)
            {
                var saleItem = new SaleItem
                {
                    Product = itemCommand.Product,
                    Quantity = itemCommand.Quantity,
                    UnitPrice = itemCommand.UnitPrice,
                    Discount = itemCommand.Discount,
                    Total = itemCommand.Total,
                    Status = itemCommand.Status
                };

                if (itemCommand.Id.HasValue && itemCommand.Id.Value != Guid.Empty)
                {
                    saleItem.Id = itemCommand.Id.Value;
                }
                

                sale.Items.Add(saleItem);
            }
        }

        // Persist updates
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Map to result DTO
        var result = _mapper.Map<ModifySaleResult>(sale);

        // Log and publish event
        _logger.LogInformation("Event: SaleUpdated - SaleNumber: {SaleNumber}", sale.SaleNumber);
        await _mediator.Publish(new SaleModifiedEvent(sale.Id, sale.SaleNumber), cancellationToken);

        return result;
    }

}