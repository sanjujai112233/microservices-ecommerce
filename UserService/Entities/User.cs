using System.ComponentModel.DataAnnotations;

namespace UserService.Entities;
class User
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public int RoleId { get; set; }
    public string? Role { get; set; }
}