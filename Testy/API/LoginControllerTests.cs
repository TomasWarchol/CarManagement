using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using CarManagementApp.API;
using CarManagementApp.Models;
using CarManagementApp.Services.JWT;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;
using LogowanieModel = CarManagementApp.Models.LogowanieModel;

namespace Testy.API;

public class LoginControllerTests
{
    private static CarManagementContext CreateCtx()
    {
        var options = new DbContextOptionsBuilder<CarManagementContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new CarManagementContext(options);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
    {
        await using var ctx = CreateCtx();
        var jwt = Substitute.For<IJwtGenerator>();
        var hasher = new PasswordHasher<Kierowca>();
        var sut = new LoginController(jwt, ctx, hasher);

        var model = new LogowanieModel { Email = "no@user.local", Haslo = "Haslo123!" };

        var result = await sut.Login(model, CancellationToken.None);

        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordInvalid()
    {
        await using var ctx = CreateCtx();
        var hasher = new PasswordHasher<Kierowca>();
        var user = new Kierowca
        {
            Email = "user@test.local",
            Imie = "Ala",
            Nazwisko = "Test",
            HasloHash = hasher.HashPassword(null!, "Correct#123")
        };
        ctx.Kierowcy.Add(user);
        await ctx.SaveChangesAsync();

        var jwt = Substitute.For<IJwtGenerator>();
        var sut = new LoginController(jwt, ctx, hasher);
        var model = new LogowanieModel { Email = "user@test.local", Haslo = "WrongPass" };

        var result = await sut.Login(model, CancellationToken.None);

        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenCredentialsValid()
    {
        await using var ctx = CreateCtx();
        var hasher = new PasswordHasher<Kierowca>();

        var user = new Kierowca
        {
            Email = "valid@test.local",
            Imie = "Jan",
            Nazwisko = "Nowak",
            HasloHash = hasher.HashPassword(null!, "Haslo123!")
        };
        ctx.Kierowcy.Add(user);
        await ctx.SaveChangesAsync();

        var jwt = Substitute.For<IJwtGenerator>();
        jwt.CreateToken(Arg.Any<Kierowca>(), Arg.Any<IEnumerable<System.Security.Claims.Claim>>())
           .Returns(("FAKE_TOKEN", DateTimeOffset.UtcNow.AddHours(2)));

        var sut = new LoginController(jwt, ctx, hasher);
        var model = new LogowanieModel { Email = "valid@test.local", Haslo = "Haslo123!" };

        var result = await sut.Login(model, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result.Result!;
        ok.Value.Should().BeAssignableTo<LoginController.TokenResponse>();
        var dto = (LoginController.TokenResponse)ok.Value!;
        dto.AccessToken.Should().Be("FAKE_TOKEN");
        dto.ExpiresAtUtc.Should().BeAfter(DateTimeOffset.UtcNow);
    }

    [Fact]
    public async Task Login_ReturnsBadRequest_WhenModelInvalid()
    {
        await using var ctx = CreateCtx();
        var jwt = Substitute.For<IJwtGenerator>();
        var hasher = new PasswordHasher<Kierowca>();
        var sut = new LoginController(jwt, ctx, hasher);

        var model = new LogowanieModel { Email = "", Haslo = "" };
        sut.ModelState.AddModelError("Email", "Required");

        var result = await sut.Login(model, CancellationToken.None);

        result.Result.Should().BeOfType<ObjectResult>();
        var obj = (ObjectResult)result.Result!;
        obj.StatusCode.Should().Be(400);
    }
}