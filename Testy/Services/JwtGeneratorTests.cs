using CarManagementApp.Models;
using CarManagementApp.Services.JWT;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Xunit;

namespace Testy.Services;

public class JwtGeneratorTests
{
    [Fact]
    public void CreateToken_ReturnsTokenAndExpiry()
    {
        // arrange
        var settings = new Dictionary<string, string?>
        {
            ["API:Secret"] = "SuperSekretnyKluczJwt_1234567890!!",
            ["API:Issuer"] = "TestIssuer",
            ["API:Audience"] = "TestAudience"
        };
        var config = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        var generator = new JwtGenerator(config);
        var user = new Kierowca { Id = 5, Email = "user@test.local", Imie = "Jan", Nazwisko = "Test" };
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id.ToString()) };

        // act
        var (token, expires) = generator.CreateToken(user, claims);

        // assert
        token.Should().NotBeNullOrWhiteSpace();
       
    }
}