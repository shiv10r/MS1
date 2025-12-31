using MS1.OrderService.Events;

namespace MS1.InventoryService.Services;

public class InventoryService
{
    public void UpdateStock(OrderCreatedEvent orderEvent)
    {
        Console.WriteLine($"[InventoryService] Reducing stock for {orderEvent.Order.ProductName}, Qty: {orderEvent.Order.Quantity}");
    }
}
