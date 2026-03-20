using OrderService.DTOs;

namespace OrderService.Dtos;

public class OrderREsponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public List<OrderItemResponseDto> Items { get; set; }

}