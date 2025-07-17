using Ambev.DeveloperEvaluation.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.Application.Sales.Cancel;

public class CancelSaleResult
{
    public string? SaleNumber { get; set; }
    public SaleStatus Status { get; set; } = SaleStatus.Active; 
}
