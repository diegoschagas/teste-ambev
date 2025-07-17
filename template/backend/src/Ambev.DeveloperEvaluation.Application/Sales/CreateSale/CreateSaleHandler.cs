using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
// No other changes are needed in the file.
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateSaleCommandValidator().ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (command.Items != null && command.Items.Any(i => i.Quantity > 20))
            throw new InvalidOperationException("Cannot sell more than 20 identical items");

        var sale = _mapper.Map<Sale>(command);

        // Calculate Total per Item if not coming from JSON
        foreach (var item in sale.Items)
        {
            item.Total = (item.Quantity * item.UnitPrice) - item.Discount;
        }

        sale.TotalAmount = sale.Items.Sum(i => i.Total);

        var addResult = await _saleRepository.AddAsync(sale, cancellationToken);

        var result = _mapper.Map<CreateSaleResult>(sale);

        _logger.LogInformation("Event: SaleCreated - SaleNumber: {SaleNumber}", sale.SaleNumber);

        return result;
    }

}
