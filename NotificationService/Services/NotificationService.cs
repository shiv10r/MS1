using MS1.OrderService.Events;

namespace MS1.NotificationService.Services;

public class NotificationService
{
    public void SendNotification(OrderCreatedEvent orderEvent)
    {
        Console.WriteLine($"[NotificationService] Notifying: Order {orderEvent.Order.Id} created for {orderEvent.Order.ProductName}");
    }
}
