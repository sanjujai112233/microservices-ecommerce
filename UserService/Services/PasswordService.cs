namespace UserService.Services;
 
public class PasswordService
{
    public string HashPassword(string Password)
    {
        return BCrypt.Net.BCrypt.HashPassword(Password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password,hash);
    }

    
}