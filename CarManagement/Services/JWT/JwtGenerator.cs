using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using CarManagement.Models;

namespace CarManagement.Services.JWT;

public class JwtGenerator : IJwtGenerator
{
    private readonly SymmetricSecurityKey key;
    public JwtGenerator(IConfiguration config)
    {
        key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["API:Secret"] ?? throw new InvalidOperationException("API:Secret configuration is missing.")));
    }

    public string CreateToken(PracownikAdministracyjny user)
    {
        var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.NameId, user.Email)
            };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(15),
            SigningCredentials = creds,
            Issuer = "HRSystem",
            Audience = "HRSystemClient"
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // public RefreshToken GenerateRefreshToken()
    // {
    //     var randomNumber = new byte[32];
    //     using var rng = RandomNumberGenerator.Create();
    //     rng.GetBytes(randomNumber);
    //     return new RefreshToken
    //     {
    //         Token = Convert.ToBase64String(randomNumber)
    //     };
    // }
}