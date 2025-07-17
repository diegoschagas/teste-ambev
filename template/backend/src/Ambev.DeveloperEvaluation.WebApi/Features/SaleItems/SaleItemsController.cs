using Ambev.DeveloperEvaluation.Application.SaleItem.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.Cancel;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleItemsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SaleItemsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("{saleId}/items/{itemId}/cancel")]
        [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSaleItem([FromRoute] Guid saleId, [FromRoute] Guid itemId, CancellationToken cancellationToken)
        {
            var command = new CancelSaleItemCommand(saleId, itemId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<CancelSaleItemResponse>
            {
                Success = true,
                Message = "Sale item cancelled successfully",
                Data = _mapper.Map<CancelSaleItemResponse>(response)
            });
        }
    }
}
