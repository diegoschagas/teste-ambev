using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Customer { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Branch { get; set; } = string.Empty;
    public List<SaleItem> Items { get; set; } = new List<SaleItem>();
    public SaleStatus Status { get; set; } = SaleStatus.Active; 

    public Sale() { }

    public Sale(string saleNumber, DateTime date, string customer, string branch, SaleStatus status, List<SaleItem> items)
    {
        SaleNumber = saleNumber;
        Date = date;
        Customer = customer;
        Branch = branch;
        Items = items;
        Status = status;
    }
}