using Microsoft.EntityFrameworkCore;
using OrderService.Entites;

namespace OrderService.Data;

public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Order> Orders{ get; set; }
    public DbSet<OrderItem> OrderItems{ get; set; }

    
} 