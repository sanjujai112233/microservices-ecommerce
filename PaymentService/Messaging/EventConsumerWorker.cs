namespace PaymentService.Messaging;

public class EventConsumerWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EventConsumerWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventConsumer(_scopeFactory);
        consumer.Start();

        return Task.CompletedTask;
    }
    
}