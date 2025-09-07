using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarManagement.Models;

namespace CarManagement.Controllers
{
    [AllowAnonymous]
    public class RejestracjaController : Controller
    {
        private readonly CarManagementContext _context;
        private readonly IPasswordHasher<Kierowca> _passwordHasher;

        public RejestracjaController(CarManagementContext context, IPasswordHasher<Kierowca> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Rejestracja()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rejestracja(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // SprawdŸ, czy u¿ytkownik o danym e-mailu ju¿ istnieje
            var existingUser = _context.Kierowcy.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Konto o podanym adresie e-mail ju¿ istnieje.");
                return View(model);
            }

            var newUser = new Kierowca
            {
                Imie = model.Imie,
                Nazwisko = model.Nazwisko,
                Email = model.Email,
            };

            // Hashowanie has³a
            newUser.HasloHash = _passwordHasher.HashPassword(newUser, model.Haslo);

            _context.Kierowcy.Add(newUser);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Rejestracja zakoñczona sukcesem. Mo¿esz siê teraz zalogowaæ.";
            return RedirectToAction("Logowanie", "Logowanie");
        }
    }
}