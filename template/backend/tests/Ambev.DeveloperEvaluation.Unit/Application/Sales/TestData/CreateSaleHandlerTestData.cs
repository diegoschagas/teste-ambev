using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
/// Gera dados de teste para comandos de criação de venda (CreateSaleCommand)
/// obedecendo as regras de negócio de quantidade e desconto.
/// </summary>
public static class CreateSaleHandlerTestData
{
    private static readonly Faker<CreateSaleItemDto> saleItemFaker = new Faker<CreateSaleItemDto>()
        .RuleFor(i => i.Product, f => $"P{f.Random.Number(100, 999)}")
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(5, 500))
        // Quantity e Discount serão calculados conforme regra abaixo, não gerados aleatoriamente
        ;

    /// <summary>
    /// Gera um CreateSaleCommand válido com um item que respeita as regras:
    /// - Quantidade entre 1 e 20
    /// - Desconto conforme quantidade
    /// </summary>
    /// <param name="quantity">Quantidade do item</param>
    /// <param name="unitPrice">Preço unitário do item</param>
    /// <returns>CreateSaleCommand configurado</returns>
    public static CreateSaleCommand GenerateValidCommand(int quantity, decimal unitPrice)
    {
        // Calcular desconto conforme regra de negócio
        decimal discount = CalculateDiscount(quantity, unitPrice);

        var item = new CreateSaleItemDto
        {
            Product = $"P{new Faker().Random.Number(100, 999)}",
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = discount
        };

        return new CreateSaleCommand
        {
            SaleNumber = $"S{new Faker().Random.Number(1000, 9999)}",
            Date = System.DateTime.Today,
            Customer = "Customer Test",
            Branch = "Branch Test",
            Items = new List<CreateSaleItemDto> { item }
        };
    }

    /// <summary>
    /// Calcula o desconto absoluto conforme as regras de negócio
    /// </summary>
    private static decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        if (quantity < 4)
            return 0m;
        if (quantity >= 4 && quantity < 10)
            return quantity * unitPrice * 0.10m;
        if (quantity >= 10 && quantity <= 20)
            return quantity * unitPrice * 0.20m;

        // Para cima de 20 não deve gerar comando válido (ou lance exceção)
        throw new System.ArgumentException("Quantity above max limit (20)");
    }
}