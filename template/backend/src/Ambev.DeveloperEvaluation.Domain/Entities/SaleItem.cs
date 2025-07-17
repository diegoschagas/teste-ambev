using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public required string Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public SaleStatus Status { get; set; } = SaleStatus.Active; 
    public SaleItem() { }

    public SaleItem(string product, int quantity, decimal unitPrice, decimal discount, SaleStatus status)
    {
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        Total = CalculateTotal();
        Status = status;
    }

    private decimal CalculateTotal() => (Quantity * UnitPrice) - Discount;
}