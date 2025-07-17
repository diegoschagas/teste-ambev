using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.SearchSale;

public class SearchSalesRequestValidator : AbstractValidator<SearchSalesRequest>
{
    public SearchSalesRequestValidator()
    {
        // Example validations
        RuleFor(x => x.SaleNumber).MaximumLength(50);
        RuleFor(x => x.Customer).MaximumLength(100);
        RuleFor(x => x.Branch).MaximumLength(50);
    }
}
