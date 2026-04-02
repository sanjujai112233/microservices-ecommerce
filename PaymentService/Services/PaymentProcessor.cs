using PaymentService.Data;
using PaymentService.Models;

namespace PaymentService.Services;

public class PaymentProcessor
{
    private readonly AppDbContext _context;
    private static readonly Random _random = new();

    public PaymentProcessor(AppDbContext context)
    {
        _context = context;
    }

    public async Task ProcessPaymentAsync(int orderId, int userId, decimal amount)
    {
        
        using var dbTransection = await _context.Database.BeginTransactionAsync();
        try
        {
            var payment = new Payment
            {
                OrderId = orderId,
                UserId = userId,
                Amount = amount,
                Status = "Pending"
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var isSuccess = _random.Next(0,2) == 1;

            var transaction = new Transaction
            {
                PaymentId = payment.Id,
                TransactionId = Guid.NewGuid().ToString(),
                Status = isSuccess ? "Success" : "Failed"
            };
            _context.Transactions.Add(transaction);
            payment.Status = transaction.Status;

            await _context.SaveChangesAsync();
            await dbTransection.CommitAsync();

            Console.WriteLine($"Payment {(isSuccess ? "Sucess" : "Failed")} for Order {orderId} ");
        }
        catch(Exception ex)
        {
            await dbTransection.RollbackAsync();
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}