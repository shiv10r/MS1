using MS1.OrderService.Models;
using MS1.OrderService.Repositories;
using MS1.OrderService.Events;

namespace MS1.OrderService.Services;

public class OrderManager
{
    private readonly OrderRepository _repository;

    // Event delegate for real-time communication
    public event Action<OrderCreatedEvent>? OrderCreated;

    public OrderManager(OrderRepository repository)
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

        // Emit event
        var orderEvent = new OrderCreatedEvent(order);
        OrderCreated?.Invoke(orderEvent);

        Console.WriteLine($"[OrderService] OrderCreated: {System.Text.Json.JsonSerializer.Serialize(order)}");

        return order;
    }
}
