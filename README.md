🔄 Project Flow (Event-Driven Microservices)

Order API

Client creates an order via REST API.

Order is stored using Repository pattern.

Event Publishing

After creation, an OrderCreatedEvent is published to RabbitMQ.

RabbitMQ enables asynchronous communication.

Event Consumption

Payment Service → Processes payment

Inventory Service → Updates stock

Notification Service → Sends notifications

Loose Coupling

Services do not call each other directly.

Each service works independently and can scale separately.


               ┌──────────────┐
               │  API / Client│
               └──────┬───────┘
                      │ HTTP Requests
                      ▼
               ┌──────────────┐
               │ OrderService │
               │  (OrderManager)│
               └──────┬───────┘
                      │ Publishes Orders
                      ▼
               ┌──────────────┐
               │  RabbitMQ    │
               │  Exchange    │
               └──────┬───────┘
          ┌───────────┼───────────┐
          ▼           ▼           ▼
 ┌──────────────┐ ┌──────────────┐ ┌───────────────┐
 │PaymentService│ │InventoryService│ │NotificationSvc│
 │   Process    │ │ Update Stock  │ │ Send Notify   │
 └──────────────┘ └──────────────┘ └───────────────┘



✅ Key Highlights

Event-driven microservice architecture

Asynchronous communication using RabbitMQ

High scalability and fault isolation

Clean separation of responsibilities
