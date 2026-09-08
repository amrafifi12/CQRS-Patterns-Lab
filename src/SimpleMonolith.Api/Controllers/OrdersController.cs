using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleMonolith.Api.Application.Order.Commands.CreateOrder;
using SimpleMonolith.Api.Application.Orders.Queries.GetOrder;

namespace SimpleMonolith.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ISender _sender;
        public OrdersController(ISender sender)
        {
            _sender = sender;
        }
        [HttpPost]
        public async Task<ActionResult<int>> Create(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
        {
            var orderId = await _sender.Send(
                command,
                cancellationToken);

            return Ok(orderId);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> Get(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetOrderQuery(id),
                cancellationToken);

            return result is null
                ? NotFound()
                : Ok(result);
        }
    }
}
