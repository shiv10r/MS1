using MS1.OrderService.Events;

namespace MS1.PaymentService.Services;

public class PaymentService
{
    public void ProcessPayment(OrderCreatedEvent orderEvent)
    {
        Console.WriteLine($"[PaymentService] Processing payment for Order {orderEvent.Order.Id}, Amount: {orderEvent.Order.Price}");
    }
}
