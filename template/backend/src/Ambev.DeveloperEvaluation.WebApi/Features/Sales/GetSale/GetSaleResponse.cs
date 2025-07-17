using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class GetSaleResponse
{
    public Guid? Id { get; set; }
    public string? SaleNumber { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string? Customer { get; set; } = string.Empty;
    public decimal? TotalAmount { get; set; }
    public string? Branch { get; set; } = string.Empty;
    public SaleStatus Status { get; set; } = SaleStatus.Active; 
    
    public string StatusDescription { get; set; } = string.Empty; 

    public List<CreateSaleItemDto>? Items { get; set; }
}
