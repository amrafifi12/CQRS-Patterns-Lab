namespace SimpleSeparateDb.Api.Domain.Models
{
    public class OrderReadModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
    }
}
