using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ForumApp.Entities;
using ForumApp.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ForumApp.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(ApplicationUser user)
    {
        var key = _configuration["Jwt:Key"] ?? throw new Exception("JWT Key bulunamadı.");

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationMinutes");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new(ClaimTypes.Name,user.UserName!),
            new(ClaimTypes.Email,user.Email!)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}