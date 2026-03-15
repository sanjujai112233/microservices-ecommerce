using Microsoft.EntityFrameworkCore;
using ProductService.Entities;

namespace ProductService.Data;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    
}