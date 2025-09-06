using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarManagement.Models;

namespace CarManagement.Controllers
{
    // [Authorize(Roles = "FleetManager")] // włącz, jeśli masz role
    public class KategorieController : Controller
    {
        private readonly CarManagementContext _context;
        private readonly ILogger<KategorieController> _logger;

        public KategorieController(CarManagementContext context, ILogger<KategorieController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Kategorie
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var kategorie = await _context.KategoriePojazdow
                .AsNoTracking()
                .OrderBy(k => k.Nazwa)
                .ToListAsync();

            return View("Kategorie", kategorie);
        }

        // GET: /Kategorie/Dodaj
        [HttpGet]
        public IActionResult Dodaj() => View(new KategoriaPojazdu());

        // POST: /Kategorie/Dodaj
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(KategoriaPojazdu model)
        {
            if (!ModelState.IsValid) return View(model);

            model.Nazwa = (model.Nazwa ?? string.Empty).Trim();

            // walidacja duplikatów (case-insensitive)
            var exists = await _context.KategoriePojazdow
                .AsNoTracking()
                .AnyAsync(k => k.Nazwa.ToLower() == model.Nazwa.ToLower());

            if (exists)
            {
                ModelState.AddModelError(nameof(KategoriaPojazdu.Nazwa), "Taka kategoria już istnieje.");
                return View(model);
            }

            try
            {
                _context.KategoriePojazdow.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kategoria została dodana.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Błąd zapisu kategorii");
                ModelState.AddModelError(string.Empty, GetFriendlyDbMessage(ex));
                return View(model);
            }
        }

        // GET: /Kategorie/Usun/5
        [HttpGet]
        public async Task<IActionResult> Usun(int id)
        {
            var kategoria = await _context.KategoriePojazdow
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kategoria == null) return NotFound();
            return View(kategoria);
        }

        // POST: /Kategorie/Usun/5 (potwierdzenie)
        [HttpPost, ActionName("Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzenie(int id)
        {
            var kategoria = await _context.KategoriePojazdow.FindAsync(id);
            if (kategoria == null) return NotFound();

            try
            {
                _context.KategoriePojazdow.Remove(kategoria);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kategoria została usunięta.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Błąd przy usuwaniu kategorii Id={Id}", id);
                TempData["Error"] = GetFriendlyDbMessage(ex);
                return RedirectToAction(nameof(Index));
            }
        }

        // ——— helper tłumaczący typowe błędy DB na przyjazne komunikaty ———
        private static string GetFriendlyDbMessage(DbUpdateException ex)
        {
            var msg = ex.InnerException?.Message ?? ex.Message;

            // duplikat (SQL Server/SQLite/Postgres — prosta heurystyka po treści)
            if (msg.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
                return "Wybrana kategoria już istnieje.";

            // naruszenie FK (rekord używany gdzie indziej)
            if (msg.Contains("REFERENCE", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase))
                return "Do tej kategorii przypisano co najmniej jeden pojazd. Nie można jej usunąć.";

            return "Wystąpił błąd podczas zapisywania zmian w bazie danych.";
        }
    }
}
