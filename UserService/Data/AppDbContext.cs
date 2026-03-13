using Microsoft.EntityFrameworkCore;
using UserService.Entities;

namespace UserService.Data;
class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users {get; set;}

    
}