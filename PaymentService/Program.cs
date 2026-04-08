using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Messaging;
using PaymentService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddHostedService<EventConsumerWorker>();
builder.Services.AddScoped<PaymentProcessor>();
//builder.Services.AddScoped<EventPublisher>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    
}



app.UseHttpsRedirection();
app.MapControllers();
app.Run();


