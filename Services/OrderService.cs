using MS1.Models;
using MS1.Repositories;

namespace MS1.Services
{
    public class OrderService
    {
        private readonly OrderRepository _repository;

        public OrderService(OrderRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Order> GetAllOrders() => _repository.GetAll();

        public Order? GetOrderById(int id) => _repository.GetById(id);

        public Order CreateOrder(Order order)
        {
            if (order.Quantity <= 0)
                throw new Exception("Quantity must be greater than 0");

            _repository.Add(order);

            // Real-time event simulation
            Console.WriteLine($"[Event] OrderCreated: {System.Text.Json.JsonSerializer.Serialize(order)}");

            return order;
        }
    }
}
