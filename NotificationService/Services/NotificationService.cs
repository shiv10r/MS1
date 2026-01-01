using MS1.NotificationService.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MS1.NotificationService.Services;

public class NotificationService
{
    public async Task SubscribeRabbitMQAsync(IConnection connection)
    {
        // Create channel (RabbitMQ 7.x)
        var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: "order_exchange",
            type: ExchangeType.Fanout
        );

        var queue = await channel.QueueDeclareAsync();

        await channel.QueueBindAsync(
            queue: queue.QueueName,
            exchange: "order_exchange",
            routingKey: ""
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

            if (orderEvent != null)
            {
                SendNotification(orderEvent);
            }

            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: queue.QueueName,
            autoAck: true,
            consumer: consumer
        );
    }

    private void SendNotification(OrderCreatedEvent orderEvent)
    {
        Console.WriteLine(
            $"[NotificationService] Notification sent for OrderId={orderEvent.Id}, Product={orderEvent.ProductName}"
        );
    }
}
