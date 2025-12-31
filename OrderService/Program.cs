using MS1.OrderService.Repositories;
using MS1.OrderService.Services;
using MS1.PaymentService.Services;
using MS1.InventoryService.Services;
using MS1.NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddSingleton<OrderManager>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Event subscriptions
var orderService = app.Services.GetRequiredService<OrderManager>();

var paymentService = new PaymentService();
orderService.OrderCreated += paymentService.ProcessPayment;

var inventoryService = new InventoryService();
orderService.OrderCreated += inventoryService.UpdateStock;

var notificationService = new NotificationService();
orderService.OrderCreated += notificationService.SendNotification;

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", () => "OrderService running");

// DO NOT specify URL here
app.Run();
