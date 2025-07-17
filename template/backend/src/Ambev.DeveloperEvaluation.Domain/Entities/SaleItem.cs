using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public string Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }

    [NotMapped]
    public bool IsCancelled { get; set; } = false;
    public SaleStatus Status { get; set; } = SaleStatus.Active; // default
    public SaleItem() { }

    public SaleItem(string product, int quantity, decimal unitPrice, decimal discount)
    {
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        Total = CalculateTotal();
    }

    private decimal CalculateTotal() => (Quantity * UnitPrice) - Discount;

}