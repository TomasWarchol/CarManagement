using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CarManagementApp.Models;

namespace CarManagementApp.Controllers
{
    [AllowAnonymous]
    public class LogowanieController : Controller
    {
        private readonly CarManagementContext _context;
        private readonly IPasswordHasher<Kierowca> _hasher;

        public LogowanieController(CarManagementContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<Kierowca>(); // możesz wstrzyknąć via DI, jeśli wolisz
        }

        [HttpGet]
        public IActionResult Logowanie()
        {
            return View(new LogowanieViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logowanie(LogowanieViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var normalizedEmail = model.Email.Trim().ToLowerInvariant();

            var user = await _context.Kierowcy
                .Include(u => u.Uprawnienia)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Nieprawidłowy e-mail lub hasło");
                return View(model);
            }

            var verify = _hasher.VerifyHashedPassword(user, user.HasloHash, model.Haslo);
            if (verify != PasswordVerificationResult.Success &&
                verify != PasswordVerificationResult.SuccessRehashNeeded)
            {
                ModelState.AddModelError(string.Empty, "Nieprawidłowy e-mail lub hasło");
                return View(model);
            }

            // Aktywne przydziały pojazdów na dziś
            var today = DateTime.UtcNow.Date;
            var aktywneAuta = await _context.Przypisania
                .AsNoTracking()
                .Where(p => p.KierowcaId == user.Id
                            && p.DataOd <= today
                            && (p.DataDo == null || p.DataDo >= today))
                .Select(p => p.Pojazd.NumerRejestracyjny)
                .ToListAsync();

            // Budujemy claims — lekko, bez wsadzania całych list do ciasteczka
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Imie),
                new Claim(ClaimTypes.Surname, user.Nazwisko),
            };

            // Rola/uprawnienia jako standardowy claim (opcjonalnie)
            if (!string.IsNullOrWhiteSpace(user.Uprawnienia?.Nazwa))
                claims.Add(new Claim(ClaimTypes.Role, user.Uprawnienia!.Nazwa));

            // Ewentualnie: liczba aut jako szybki claim (pełną listę pobieraj z bazy przy potrzebie)
            claims.Add(new Claim("fleet:vehicles_count", aktywneAuta.Count.ToString()));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProps = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(14) : (DateTimeOffset?)null
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

            TempData["Success"] = "Zalogowano pomyślnie!";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Wyloguj()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "Wylogowano pomyślnie!";
            return RedirectToAction("Index", "Home");
        }
    }
}
