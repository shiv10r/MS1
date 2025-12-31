using MS1.OrderService.Events;
using MS1.OrderService.Models;
using MS1.OrderService.Repositories;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;


namespace MS1.OrderService.Services
{
    public class OrderManager
    {
        private readonly OrderRepository _repo;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public event Action<OrderCreatedEvent>? OrderCreated;

        public OrderManager(OrderRepository repo)
        {
            _repo = repo;

            // Setup RabbitMQ connection
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "orders", durable: false, exclusive: false, autoDelete: false);
        }

        public Order CreateOrder(Order order)
        {
            var created = _repo.Add(order);

            var orderEvent = new OrderCreatedEvent
            {
                Id = created.Id,
                ProductName = created.ProductName,
                Quantity = created.Quantity,
                Price = created.Price,
                CreatedAt = created.CreatedAt
            };

            // Trigger in-memory events
            OrderCreated?.Invoke(orderEvent);

            // Publish to RabbitMQ
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(orderEvent));
            _channel.BasicPublish(exchange: "", routingKey: "orders", basicProperties: null, body: body);

            return created;
        }

        public IEnumerable<Order> GetAllOrders() => _repo.GetAll();
        public Order? GetOrderById(int id) => _repo.GetById(id);
    }
}
