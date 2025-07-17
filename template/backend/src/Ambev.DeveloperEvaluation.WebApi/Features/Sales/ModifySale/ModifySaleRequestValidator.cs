using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ModifySale;

public class ModifySaleRequestValidator : AbstractValidator<ModifySaleRequest>
{
    public ModifySaleRequestValidator()
    {
        RuleFor(x => x.SaleNumber).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Customer).NotEmpty();
        RuleFor(x => x.Branch).NotEmpty();

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one sale item is required.")
            .Custom((items, context) =>
            {
                foreach (var item in items)
                {
                    if (string.IsNullOrWhiteSpace(item.Product))
                    {
                        context.AddFailure("Items", "Product is required for each item.");
                    }

                    if (item.Quantity <= 0)
                    {
                        context.AddFailure($"Item '{item.Product}' must have quantity greater than zero.");
                    }

                    if (item.Quantity > 20)
                    {
                        context.AddFailure($"Item '{item.Product}' cannot have quantity above 20.");
                    }

                    if (item.Quantity < 4)
                    {
                        if (item.Discount > 0)
                        {
                            context.AddFailure($"Item '{item.Product}' cannot have a discount for quantities below 4.");
                        }
                    }
                    else if (item.Quantity >= 4 && item.Quantity < 10)
                    {
                        var expectedDiscount = CalculateDiscount(item.UnitPrice, item.Quantity, 10);
                        if (item.Discount != expectedDiscount)
                        {
                            context.AddFailure($"Item '{item.Product}' must have a 10% discount (expected {expectedDiscount:C}) for quantities between 4 and 9.");
                        }
                    }
                    else if (item.Quantity >= 10 && item.Quantity <= 20)
                    {
                        var expectedDiscount = CalculateDiscount(item.UnitPrice, item.Quantity, 20);
                        if (item.Discount != expectedDiscount)
                        {
                            context.AddFailure($"Item '{item.Product}' must have a 20% discount (expected {expectedDiscount:C}) for quantities between 10 and 20.");
                        }
                    }
                }
            });
    }

    private decimal CalculateDiscount(decimal unitPrice, int quantity, decimal discountPercentage)
    {
        var totalWithoutDiscount = unitPrice * quantity;
        return totalWithoutDiscount * (discountPercentage / 100);
    }
}
