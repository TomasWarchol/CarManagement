using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CarManagementApp.Models;

namespace CarManagementApp.Services.JWT;

public class JwtGenerator : IJwtGenerator
{
    // The signing key is never stored in appsettings.json. Locally it comes from user-secrets,
    // on a server from an environment variable (API__Secret) or a key vault.
    public const string MissingSecretMessage =
        "API:Secret is not configured. For local development run: " +
        "dotnet user-secrets set \"API:Secret\" \"<random key, at least 64 characters>\" --project CarManagement";

    private readonly SymmetricSecurityKey key;
    public JwtGenerator(IConfiguration config)
    {
        key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["API:Secret"] ?? throw new InvalidOperationException(MissingSecretMessage)));
    }

    public (string token, DateTimeOffset expiresAtUtc) CreateToken(Kierowca user, IEnumerable<Claim> claims)
    {
        var allClaims = new List<Claim>(claims)
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Email)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var expires = DateTimeOffset.UtcNow.AddDays(15);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(allClaims),
            Expires = expires.UtcDateTime,
            SigningCredentials = creds,
            Issuer = "CarSystem",
            Audience = "CarSystemClient"
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (tokenHandler.WriteToken(token), expires);
    }
}