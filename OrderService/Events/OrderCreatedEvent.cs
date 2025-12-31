namespace MS1.OrderService.Events
{
    public class OrderCreatedEvent
    {
        public int Id { get; set; }            // Order Id
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
