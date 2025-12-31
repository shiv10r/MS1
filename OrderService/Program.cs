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
app.Run();
// Fix for CS0234: Ensure the correct namespace is used for InventoryService.
// If InventoryService is in a different namespace, update the using statement accordingly.
// For example, if the correct namespace is MS1.Inventory.Services, change the using statement to:


// If you are unsure of the correct namespace, please provide the definition or location of InventoryService.
