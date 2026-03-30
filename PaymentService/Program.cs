using PaymentService.Messaging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHostedService<PaymentService.Messaging.EventConsumerWorker>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    
}



app.UseHttpsRedirection();
app.MapControllers();
app.Run();


