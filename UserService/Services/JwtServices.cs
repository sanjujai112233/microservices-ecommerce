using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UserService.Services;

public class JwtServices
{
    private readonly IConfiguration _config;
    public JwtServices(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateTokkern(string email, string role)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"])
        );
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new []
        {
          new Claim(ClaimTypes.Email, email),  
          new Claim(ClaimTypes.Role, role),  
        };

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience : _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                Convert.ToDouble(_config["JwtSettings:ExpiryMinutes"])),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}