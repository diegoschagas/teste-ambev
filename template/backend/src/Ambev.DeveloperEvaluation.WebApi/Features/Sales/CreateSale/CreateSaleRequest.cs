using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public string? SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public string? Customer { get; set; }
        public string? Branch { get; set; }
        public List<CreateSaleItemDto>? Items { get; set; }
    }
}
