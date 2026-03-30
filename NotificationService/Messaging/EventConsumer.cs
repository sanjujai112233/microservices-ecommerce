using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService.Messaging;

public class EventConsumer
{
    public void Start()
    {
        Console.WriteLine("🔔 Notification Consumer Started...");
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "payment_success_queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            Console.WriteLine("📥 Notification Received!");
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[NotificationService] Received: {message}");


            Console.WriteLine("📩 Sending notification...");
            Thread.Sleep(500);
            Console.WriteLine("Notification Sent");
        };

        channel.BasicConsume(
            queue: "payment_success_queue",
            autoAck: true,
            consumer: consumer
        );

    }
}