using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public string Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public bool IsCancelled { get; set; }

    public SaleItem() { }

    public SaleItem(string product, int quantity, decimal unitPrice, decimal discount)
    {
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        Total = CalculateTotal();
        IsCancelled = false;
    }

    private decimal CalculateTotal() => (Quantity * UnitPrice) - Discount;

}