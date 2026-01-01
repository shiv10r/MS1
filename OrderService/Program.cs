using MS1.OrderService.Repositories;
using MS1.OrderService.Services;
using MS1.PaymentService.Services;
using MS1.InventoryService.Services;
using MS1.NotificationService.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Register application services
builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddSingleton<OrderManager>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -------------------------------
// RabbitMQ connection (7.x async)
// -------------------------------
var factory = new ConnectionFactory
{
    HostName = "localhost"
};

var rabbitConnection = await factory.CreateConnectionAsync();

// -------------------------------
// Start RabbitMQ consumers
// -------------------------------
var paymentService = new PaymentService();
await paymentService.SubscribeRabbitMQAsync(rabbitConnection);

var inventoryService = new InventoryService();
await inventoryService.SubscribeRabbitMQAsync(rabbitConnection);

var notificationService = new NotificationService();
await notificationService.SubscribeRabbitMQAsync(rabbitConnection);

// -------------------------------
// Middleware
// -------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => "OrderService running");

app.Run();
