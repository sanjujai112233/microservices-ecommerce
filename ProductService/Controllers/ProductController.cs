using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProductService.Data;
using ProductService.DTOs;
using ProductService.Entities;

namespace ProductService.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;

    public ProductController(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        if (!_cache.TryGetValue("product_list", out List<Product> products))
        {
            products = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.Inventory)
                .ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set("product_list", products, cacheOptions);
        }
        var result = products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Category = p.Category.Name,
            Quantity = p.Inventory.Quantity,

        });

        return Ok(result);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetProduct(int Id)
    {
        var product = await _context.Products
        .Include(x => x.Category)
        .Include(x => x.Inventory)
        .FirstOrDefaultAsync(x => x.Id == Id);

        if (product == null)
            return NotFound();

        var result = new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category.Name,
            Quantity = product.Inventory.Quantity,

        };
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var Inventory = new Inventory
        {
            ProductId = product.Id,
            Quantity = dto.Quantity,
        };

        _context.Inventories.Add(Inventory);
        await _context.SaveChangesAsync();

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;

        var inventory = await _context.Inventories.FirstOrDefaultAsync(x => x.ProductId == id);

        if (inventory != null)
        {
            inventory.Quantity = dto.Quantity;
        }

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok("Product Deleted");
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts(string name)
    {
        var product = await _context.Products
        .Include(x=> x.Category)
        .Include(x=>x.Inventory)
        .Where(x => EF.Functions.Like(x.Name, $"%{name}%"))
        .ToListAsync();

         var result = product.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Category = p.Category.Name,
            Quantity = p.Inventory.Quantity,

        });
        return Ok(result);
    }
}