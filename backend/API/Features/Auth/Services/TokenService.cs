using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Features.Auth.Services.Interfaces;
using API.Features.Users.Entities;
using Microsoft.IdentityModel.Tokens;

namespace API.Features.Auth.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string CreateToken(User user)
    {
        var tokenKey = configuration["TokenKey"] ?? throw new Exception("TokenKey not found - TokenService.cs");

        if (tokenKey.Length < 64)
            throw new Exception("TokenKey must be >= 64 characters - TokenService.cs");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}