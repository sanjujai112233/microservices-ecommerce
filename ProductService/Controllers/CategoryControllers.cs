using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductService.Data;
using ProductService.DTOs;
using ProductService.Entities;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryControllers : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryControllers(AppDbContext context)
    {
        _context = context;
    }


    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name
        };
         _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return Ok(category);
    }

    [HttpGet]
    public IActionResult GetCategory()
    {
        return Ok(_context.Categories.ToList());
    }
    
}