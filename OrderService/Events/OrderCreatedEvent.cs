using MS1.OrderService.Models;

namespace MS1.OrderService.Events;

public record OrderCreatedEvent(Order Order);
