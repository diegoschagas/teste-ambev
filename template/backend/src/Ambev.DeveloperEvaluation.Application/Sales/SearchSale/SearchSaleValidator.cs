using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.SearchSale;

public class SearchSaleValidator : AbstractValidator<SearchSaleCommand>
{
    public SearchSaleValidator()
    {
        RuleFor(x => x.SaleNumber);
    }
}
