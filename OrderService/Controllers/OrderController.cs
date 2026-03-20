using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.DTOs;
using OrderService.Services;
using OrderService.Entites;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using OrderService.Dtos;
namespace OrderService.Controllers;


[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ProductServiceClient _productClient;

    public OrderController(AppDbContext context, ProductServiceClient productClient)
    {
        _context = context;
        _productClient = productClient;
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        // ✅ Step 1: Validate products FIRST
        foreach (var item in dto.Items)
        {
            var productExist = await _productClient.ProductExists(item.ProductId);

            if (!productExist)
                return BadRequest($"Product {item.ProductId} doesn't exist");
        }

        // ✅ Step 2: Create Order
        var order = new Order
        {
            UserId = dto.UserId,
            OrderDate = DateTime.UtcNow,
            Satus = "Pending"
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // ✅ Step 3: Create OrderItems
        var orderItems = new List<OrderItem>();

        foreach (var item in dto.Items)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantitiy = item.Quantity,
                Price = 0
            };

            orderItems.Add(orderItem);
            _context.OrderItems.Add(orderItem);
        }

        await _context.SaveChangesAsync();

        // ✅ Step 4: MAP TO DTO (IMPORTANT 🔥)
        var response = new OrderREsponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            Status = order.Satus,
            Items = orderItems.Select(i => new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantitiy,
                Price = i.Price
            }).ToList()
        };

        // ✅ Step 5: Return clean response
        return Ok(response);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUSerOrders(int userId)
    {
        var orders = await _context.Orders
        .Include(x => x.OrderItems)
        .Where(x => x.UserId == userId).ToListAsync();

        var response = orders.Select(order => new OrderREsponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            Status = order.Satus,
            Items = order.OrderItems.Select(item => new OrderItemResponseDto
            {
                ProductId = item.ProductId,
                Quantity = item.Quantitiy,
                Price = item.Price
            }).ToList()
        });

        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrderStatus(int id, string status)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
            return NotFound();

        order.Satus = status;

        await _context.SaveChangesAsync();

        var response = new OrderREsponseDto
        {
          Id = order.Id,
          UserId = order.UserId,
          OrderDate = order.OrderDate,
          Status = order.Satus,
          Items = order.OrderItems.Select(item => new OrderItemResponseDto
          {
              ProductId = item.ProductId,
              Quantity = item.Quantitiy,
              Price = item.Price
          } ).ToList()  
        };
        return Ok(response);

    }




}