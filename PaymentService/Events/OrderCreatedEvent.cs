namespace PaymentService.Events;

public class OrderCreatedEvent
{
    public int OrderId { get; set; }
    public int UserId { get; set; }


}