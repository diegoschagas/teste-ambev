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
                status: Domain.Enums.SaleStatus.Active,
                items: new List<SaleItem>
                {
                    new SaleItem
                    {
                        Product = "Beer 350ml",
                        Quantity = 10,
                        UnitPrice = 4.50m,
                        Discount = 0,
                        Status = Domain.Enums.SaleStatus.Active
                    },
                    new SaleItem
                    {
                        Product = "Soda 2L",
                        Quantity = 2,
                        UnitPrice = 7.00m,
                        Discount = 1.00m,
                        Status = Domain.Enums.SaleStatus.Active
                    }
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
                status: Domain.Enums.SaleStatus.Active,
                items: new List<SaleItem>
                {
                    new SaleItem
                    {
                        Product = "Energy Drink",
                        Quantity = 5,
                        UnitPrice = 8.00m,
                        Discount = 0,
                        Status = Domain.Enums.SaleStatus.Active
                    }
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
