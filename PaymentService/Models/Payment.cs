namespace PaymentService.Models;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}