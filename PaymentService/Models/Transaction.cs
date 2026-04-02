namespace PaymentService.Models;

public class Transaction
{
    public int Id { get; set; }
    public int PaymentId { get; set; }
    public string  TransactionId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 