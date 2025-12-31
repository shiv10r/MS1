using MS1.OrderService.Events;

namespace MS1.InventoryService.Services;

public class InventoryService
{
    public void SubscribeRabbitMQ(RabbitMQ.Client.IConnection connection)
    {
        var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "order_exchange", type: RabbitMQ.Client.ExchangeType.Fanout);
        var queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(queue: queueName, exchange: "order_exchange", routingKey: "");

        var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var orderEvent = System.Text.Json.JsonSerializer.Deserialize<OrderCreatedEvent>(body);
            if (orderEvent != null)
                UpdateStock(orderEvent);
        };
        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }

    public void UpdateStock(OrderCreatedEvent orderEvent)
    {
        Console.WriteLine($"[InventoryService] Stock updated for order {orderEvent.OrderId}, Product: {orderEvent.ProductName}, Quantity: {orderEvent.Quantity}");
    }
}
