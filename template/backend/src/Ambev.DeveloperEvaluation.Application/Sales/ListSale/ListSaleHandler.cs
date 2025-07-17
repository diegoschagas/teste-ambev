using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.SearchSale;

public class ListSaleHandler : IRequestHandler<ListSaleCommand, List<ListSaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public ListSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<List<ListSaleResult>> Handle(ListSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new ListSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sales = await _saleRepository.GetAllAsync(cancellationToken);
        if (sales == null || !sales.Any())
            throw new KeyNotFoundException("Sales not found");

        var saleList = _mapper.Map<List<ListSaleResult>>(sales);


        return saleList;


    }
}
