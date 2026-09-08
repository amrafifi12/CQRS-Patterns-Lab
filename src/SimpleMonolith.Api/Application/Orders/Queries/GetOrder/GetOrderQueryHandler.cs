using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleMonolith.Api.Application.Abstractions.Persistence;

namespace SimpleMonolith.Api.Application.Orders.Queries.GetOrder
{
    public sealed class GetOrderQueryHandler
    : IRequestHandler<GetOrderQuery, OrderDto?>
    {
        private readonly IApplicationDbContext _context;

        public GetOrderQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto?> Handle(
            GetOrderQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(order => order.Id == request.OrderId)
                .Select(order => new OrderDto(
                    order.Id,
                    order.CustomerId,
                    order.TotalAmount,
                    order.Status.ToString()))
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
