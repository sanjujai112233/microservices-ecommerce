using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Entities;
using UserService.Services;

namespace UserService.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly JwtServices _jwtServices;

    public AuthController(AppDbContext context, PasswordService passwordService, JwtServices jwtServices)
    {
        _context = context;
        _passwordService = passwordService;
        _jwtServices = jwtServices;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var hashPassword = _passwordService.HashPassword(dto.Password);

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = hashPassword,
            RoleId = 2
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if(user == null)
            return Unauthorized();

            if(!_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
            return Unauthorized();
            var token = _jwtServices.GenerateTokkern(user.Email, user.Role.Name);

            return Ok(new{token});
    }


    

    
}