using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CarManagement.Models;
using CarManagement.Services.JWT;

namespace CarManagement.API;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class LoginController(
    IJwtGenerator jwtGenerator,
    CarManagementContext context,
    IPasswordHasher<Kierowca> hasher
) : ControllerBase
{
    public sealed class TokenResponse
    {
        public string AccessToken { get; init; } = string.Empty;
        public string TokenType { get; init; } = "Bearer";
        public DateTimeOffset ExpiresAtUtc { get; init; }
    }

    /// <summary>Logowanie i wydanie tokenu JWT.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LogowanieModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        // Zamień linię:
        // var email = model.Email.Trim().ToLowerInvariant();

        // Na:
        var email = (model.Email as string)?.Trim().ToLowerInvariant() ?? string.Empty;

        var user = await context.Kierowcy
            .Include(u => u.Uprawnienia)
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email.ToLower() == email, ct);

        if (user is null)
            return Unauthorized("Nieprawidłowy e-mail lub hasło.");

        var verification = hasher.VerifyHashedPassword(user, user.HasloHash, model.Haslo);
        if (verification is not PasswordVerificationResult.Success
            and not PasswordVerificationResult.SuccessRehashNeeded)
            return Unauthorized("Nieprawidłowy e-mail lub hasło.");

        var today = DateTime.UtcNow.Date;
        var vehiclesCount = await context.Przypisania
            .Where(p => p.KierowcaId == user.Id && p.DataOd <= today && (p.DataDo == null || p.DataDo >= today))
            .AsNoTracking()
            .CountAsync(ct);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.Imie),
            new(ClaimTypes.Surname, user.Nazwisko),
            new("fleet:vehicles_count", vehiclesCount.ToString())
        };

        if (!string.IsNullOrWhiteSpace(user.Uprawnienia?.Nazwa))
            claims.Add(new Claim(ClaimTypes.Role, user.Uprawnienia!.Nazwa));

        var (token, expiresAtUtc) = jwtGenerator.CreateToken(user, claims);

        return Ok(new TokenResponse
        {
            AccessToken = token,
            ExpiresAtUtc = expiresAtUtc
        });
    }
}
