using Microsoft.EntityFrameworkCore;
using UserService.Entities;

namespace UserService.Data;
class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users {get; set;}
    public DbSet<Role> Roles {get; set;}
    public DbSet<RefreshToken> RefreshTokens {get; set;}
}