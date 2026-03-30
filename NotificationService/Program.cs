using NotificationService.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var consumer = new EventConsumer();
consumer.Start();


app.UseHttpsRedirection();


app.Run();

