using MediatR;
using SimpleSeparateDb.Api.Application.Abstractions.Persistence;
using SimpleSeparateDb.Api.Domain.Models;

namespace SimpleSeparateDb.Api.Application.Orders.Queries.GetOrder
{
    public sealed class GetOrderQuery : IRequest<OrderReadModel?>
    {
        public int Id { get; set; }

        public GetOrderQuery(int id)
        {
            Id = id;
        }
    }

    public sealed class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderReadModel?>
    {
        private readonly IReadDbContext _readDbContext;

        public GetOrderQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<OrderReadModel?> Handle(
            GetOrderQuery request,
            CancellationToken cancellationToken)
        {
            return await _readDbContext.FindAsync(request.Id, cancellationToken);
        }
    }
}
