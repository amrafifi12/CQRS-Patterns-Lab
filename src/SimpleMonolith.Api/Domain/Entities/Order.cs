using SimpleMonolith.Api.Domain.Enums;

namespace SimpleMonolith.Api.Domain.Entities
{
    public sealed class Order
    {
        public int Id { get; private set; }

        public int CustomerId { get; private set; }

        public decimal TotalAmount { get; private set; }

        public OrderStatus Status { get; private set; }

        private Order()
        {
        }

        public Order(int customerId, decimal totalAmount)
        {
            if (customerId <= 0)
                throw new ArgumentException("CustomerId must be greater than zero.");

            if (totalAmount <= 0)
                throw new ArgumentException("TotalAmount must be greater than zero.");

            CustomerId = customerId;
            TotalAmount = totalAmount;
            Status = OrderStatus.Pending;
        }

        public void Confirm()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending orders can be confirmed.");

            Status = OrderStatus.Confirmed;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException(
                    "Order is already cancelled.");

            Status = OrderStatus.Cancelled;
        }
    }
}
