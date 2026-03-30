namespace PaymentService.Messaging;

public class EventConsumerWorker : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventConsumer();
        consumer.Start();

        return Task.CompletedTask;
    }
    
}