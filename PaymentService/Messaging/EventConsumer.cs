using System.Text;
using System.Text.Json;
using PaymentService.Events;
using PaymentService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PaymentService.Messaging;

public class EventConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EventConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void Start()
    {
        Console.WriteLine("✅ Payment Consumer Started...");
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "order_created_queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            Console.WriteLine("📥 Message Received!");
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[PaymentService] Received: {message}");

            var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

            using var scope = _scopeFactory.CreateScope();
            var processor = scope.ServiceProvider.GetRequiredService<PaymentProcessor>();

            await processor.ProcessPaymentAsync(
                orderEvent.OrderId,
                orderEvent.UserId,
                amount: 1000
            );


            Console.WriteLine("💳 Processing payment...");
            Thread.Sleep(1000);
            Console.WriteLine("✅ Payment successful");

            var responseMessage  = Encoding.UTF8.GetBytes(message);


            channel.QueueDeclare(
                queue: "payment_success_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            channel.BasicPublish(
                exchange: "",
                routingKey: "payment_success_queue",
                basicProperties: null,
                body: responseMessage
            );

            Console.WriteLine("📤 Payment success event published!");
        };

        channel.BasicConsume(
            queue: "order_created_queue",
            autoAck: true,
            consumer: consumer
        );

    }
}