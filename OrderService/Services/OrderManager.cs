using MS1.OrderService.Models;
using MS1.OrderService.Repositories;
using MS1.NotificationService.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MS1.OrderService.Services
{
    public class OrderManager
    {
        private readonly OrderRepository _repo;
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        public event Action<OrderCreatedEvent>? OrderCreated;

        public OrderManager(OrderRepository repo)
        {
            _repo = repo;

            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            // ❌ DON'T assign return value
            _channel.ExchangeDeclareAsync(
                exchange: "order_exchange",
                type: ExchangeType.Fanout
            ).GetAwaiter().GetResult();
        }

        public Order CreateOrder(Order order)
        {
            _repo.Add(order);

            var orderEvent = new OrderCreatedEvent
            {
                Id = order.Id,
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                Price = order.Price,
                CreatedAt = order.CreatedAt
            };

            OrderCreated?.Invoke(orderEvent);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(orderEvent));

            // ❌ DON'T assign return value
            _channel.BasicPublishAsync(
                exchange: "order_exchange",
                routingKey: "",
                body: body
            ).GetAwaiter().GetResult();

            return order;
        }

        public IEnumerable<Order> GetAllOrders() => _repo.GetAll();
        public Order? GetOrderById(int id) => _repo.GetById(id);
    }
}
