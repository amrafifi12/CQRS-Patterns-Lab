namespace SimpleSeparateDb.Api.Application.Events
{
   public sealed record OrderCreatedEvent(
        int OrderId,
        int CustomerId,
        decimal TotalAmount,
        int Status);
}
