using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using infrastructure.QueryModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace service;

public interface ITokenService
{
    string GenerateToken(AccountQuery account);
}

public class TokenService: ITokenService
{
    private IConfiguration Configuration { get; }
    public TokenService(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }
    
    
    public string GenerateToken(AccountQuery account)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            Configuration["Jwt:Key"]!));
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Name),
            new Claim(JwtRegisteredClaimNames.Email, account.Email),
            new Claim("rank", account.Rank.ToString()),
            new Claim("id", account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        
        };
        var token = new JwtSecurityToken(
            Configuration["Jwt:Issuer"],
            Configuration["Jwt:Audience"],
            claims:claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}