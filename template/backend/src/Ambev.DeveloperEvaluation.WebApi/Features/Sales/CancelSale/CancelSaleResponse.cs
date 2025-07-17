using Ambev.DeveloperEvaluation.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public class CancelSaleResponse
{
    public string? SaleNumber { get; set; }
    public SaleStatus Status { get; set; } = SaleStatus.Active;
    public string StatusDescription { get; set; } = string.Empty;

}
