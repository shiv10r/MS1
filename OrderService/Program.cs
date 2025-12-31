using MS1.OrderService.Repositories;
using MS1.OrderService.Services;
using MS1.PaymentService.Services;
using MS1.InventoryService.Services;
using MS1.NotificationService.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddSingleton<OrderManager>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// RabbitMQ connection for subscribers
var factory = new ConnectionFactory() { HostName = "localhost" };
var connection = factory.CreateConnection();

// Subscribe services to RabbitMQ
var paymentService = new PaymentService();
paymentService.SubscribeRabbitMQ(connection);

var inventoryService = new InventoryService();
inventoryService.SubscribeRabbitMQ(connection);

var notificationService = new NotificationService();
notificationService.SubscribeRabbitMQ(connection);

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
