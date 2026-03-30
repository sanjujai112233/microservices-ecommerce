namespace OrderService.Events;

public class OrderCreatedEvents
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public List<OrderItemEvent> Items { get; set; }
}

public class OrderItemEvent
{
    public int ProductId { get; set; }
    public int Quantitiy { get; set; }
}