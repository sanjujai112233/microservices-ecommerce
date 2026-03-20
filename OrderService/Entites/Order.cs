namespace OrderService.Entites;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Satus { get; set; }
    public ICollection<OrderItem> OrderItems  { get; set; }
}