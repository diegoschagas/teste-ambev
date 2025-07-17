using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.ORM.Seed;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(DefaultContext context)
    {
        if (!context.Sales.Any())
        {
            var sale1 = new Sale(
                saleNumber: "S0001",
                date: DateTime.UtcNow.AddDays(-2),
                customer: "John Doe",
                branch: "Porto Alegre",
                items: new List<SaleItem>
                {
                    new SaleItem("Beer 350ml", 10, 4.50m, 0),
                    new SaleItem("Soda 2L", 2, 7.00m, 1.00m)
                }
            )
            {
                TotalAmount = 10 * 4.50m + 2 * 7.00m - 1.00m
            };

            var sale2 = new Sale(
                saleNumber: "S0002",
                date: DateTime.UtcNow.AddDays(-1),
                customer: "Jane Smith",
                branch: "São Paulo",
                items: new List<SaleItem>
                {
                    new SaleItem("Energy Drink", 5, 8.00m, 0)
                }
            )
            {
                TotalAmount = 5 * 8.00m
            };

            await context.Sales.AddRangeAsync(sale1, sale2);
            await context.SaveChangesAsync();
        }
    }
}
