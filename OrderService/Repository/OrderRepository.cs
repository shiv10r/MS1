using MS1.OrderService.Models;
using System.Collections.Concurrent;

namespace MS1.OrderService.Repositories;

public class OrderRepository
{
    private readonly ConcurrentDictionary<int, Order> _orders = new();

    public IEnumerable<Order> GetAll() => _orders.Values;

    public Order? GetById(int id) => _orders.TryGetValue(id, out var order) ? order : null;

    public void Add(Order order) => _orders[order.Id] = order;
}
