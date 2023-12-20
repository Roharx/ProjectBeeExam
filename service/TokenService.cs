using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using infrastructure.QueryModels;
using Microsoft.IdentityModel.Tokens;

namespace service;

public interface ITokenService
{
    string GenerateToken(AccountQuery account);
}

public class TokenService: ITokenService
{
    private const string SecretKey = "ThisIsATestSecretKeyForJWTWithMoreThan256Bits"; // Replace with secret stuff later
    private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(SecretKey)); // change later to more secure version

    public string GenerateToken(AccountQuery account)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Name),
            new Claim(JwtRegisteredClaimNames.Email, account.Email),
            new Claim("rank", account.Rank.ToString()),
            new Claim("id", account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

        };

        var token = new JwtSecurityToken(
            issuer: "your-issuer", // Replace in later patches
            audience: "your-audience", // Replace in later patches
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}