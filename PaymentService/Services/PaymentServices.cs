using MS1.NotificationService.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MS1.PaymentService.Services;

public class PaymentService
{
    public async Task SubscribeRabbitMQAsync(IConnection connection)
    {
        // ✅ RabbitMQ 7.x
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

        // ✅ New consumer
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

            if (orderEvent != null)
            {
                ProcessPayment(orderEvent);
            }

            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: queue.QueueName,
            autoAck: true,
            consumer: consumer
        );
    }

    private void ProcessPayment(OrderCreatedEvent orderEvent)
    {
        Console.WriteLine(
            $"[PaymentService] Payment processed for OrderId={orderEvent.Id}, Product={orderEvent.ProductName}"
        );
    }
}
