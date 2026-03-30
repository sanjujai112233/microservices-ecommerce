using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
namespace OrderService.Messaging;

public class EventPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public EventPublisher()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    public void PublishOrderCreated(object message)
    {
         Console.WriteLine("🔥 Publishing event to RabbitMQ...");
        _channel.QueueDeclare(
            queue: "order_created_queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: "",
            routingKey: "order_created_queue",
            basicProperties: null,
            body: body
        );
    }


}