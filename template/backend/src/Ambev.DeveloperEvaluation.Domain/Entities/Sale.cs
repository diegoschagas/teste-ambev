using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; set; }
    public DateTime Date { get; set; }
    public string Customer { get; set; }
    public decimal TotalAmount { get; set; }
    public string Branch { get; set; }
    public List<SaleItem> Items { get; set; }
    public bool IsCancelled { get; set; }

    public Sale() { }

    public Sale(string saleNumber, DateTime date, string customer, string branch, List<SaleItem> items)
    {
        SaleNumber = saleNumber;
        Date = date;
        Customer = customer;
        Branch = branch;
        Items = items;
    }
}