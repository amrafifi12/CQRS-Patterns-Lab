using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleSeparateDb.Api.Application.Orders.Commands.CreateOrder;
using SimpleSeparateDb.Api.Application.Orders.Queries.GetOrder;
using SimpleSeparateDb.Api.Domain.Models;

namespace SimpleSeparateDb.Api.Controllers
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
            var orderId = await _sender.Send(command, cancellationToken);
            return Ok(orderId);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderReadModel>> Get(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetOrderQuery(id), cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
