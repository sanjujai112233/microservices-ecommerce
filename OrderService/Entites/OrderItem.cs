namespace OrderService.Entites;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantitiy { get; set; }
    public decimal Price { get; set; }
    public Order Order { get; set; }
    
}