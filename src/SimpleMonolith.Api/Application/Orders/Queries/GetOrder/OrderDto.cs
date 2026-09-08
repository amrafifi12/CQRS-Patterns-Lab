namespace SimpleMonolith.Api.Application.Orders.Queries.GetOrder
{
    public sealed record OrderDto(
        int Id,
        int CustomerId,
        decimal TotalAmount,
        string Status);

}
