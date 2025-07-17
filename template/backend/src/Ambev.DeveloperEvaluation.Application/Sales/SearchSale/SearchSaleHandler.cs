using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.SearchSale;

public class SearchSaleHandler : IRequestHandler<SearchSaleCommand, List<Sale>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public SearchSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<List<Sale>> Handle(SearchSaleCommand request, CancellationToken cancellationToken)
    {
        return await _saleRepository.SearchAsync(
            request?.SaleNumber,
            request?.Date,
            request?.Customer,
            request?.TotalAmount,
            request?.Branch,
            cancellationToken);
    }
}
