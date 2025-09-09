using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarManagementApp.Models;
using CarManagementApp.Models.ViewModels;
using Microsoft.Extensions.Logging;

namespace CarManagementApp.Controllers
{
    [Authorize]
    public class PojazdyController : Controller
    {
        private readonly CarManagementContext _context;
        private readonly ILogger<PojazdyController> _logger;

        public PojazdyController(CarManagementContext context, ILogger<PojazdyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Pojazdy
        public async Task<IActionResult> Index()
        {
            var pojazdy = await _context.Pojazdy
                .Include(p => p.Kategoria)
                .AsNoTracking()
                .OrderBy(p => p.NumerRejestracyjny)
                .ToListAsync();
            return View(pojazdy);
        }

        // GET: /Pojazdy/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var pojazd = await _context.Pojazdy
                .Include(p => p.Kategoria)
                .Include(p => p.Polisy)
                .Include(p => p.Serwisy)
                .Include(p => p.Przypisania).ThenInclude(a => a.Kierowca)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pojazd == null) return NotFound();

            decimal? srednie = null;
            var tankowania = await _context.Tankowania
                .Where(t => t.PojazdId == id && t.IloscLitrow > 0 && t.PrzebiegKm != null)
                .OrderBy(t => t.Data)
                .AsNoTracking()
                .ToListAsync();

            if (tankowania.Count >= 2)
            {
                var first = tankowania.First();
                var last = tankowania.Last();
                var przejechane = (last.PrzebiegKm ?? 0) - (first.PrzebiegKm ?? 0);
                var litry = tankowania.Sum(t => t.IloscLitrow);
                if (przejechane > 0)
                    srednie = (litry / przejechane) * 100m;
            }

            var vm = new PojazdDetailsViewModel
            {
                Pojazd = pojazd,
                PolisyAktywne = pojazd.Polisy.Where(p => p.DataDo >= DateTime.Today).OrderBy(p => p.DataDo),
                OstatnieSerwisy = pojazd.Serwisy.OrderByDescending(s => s.Data).Take(5),
                HistoriaPrzypisan = pojazd.Przypisania.OrderByDescending(a => a.DataOd),
                SrednieSpalanieL100 = srednie.HasValue ? Math.Round(srednie.Value, 2) : null
            };

            return View(vm);
        }

        // GET: /Pojazdy/Create
        public IActionResult Create()
        {
            ViewBag.Kategorie = _context.KategoriePojazdow.AsNoTracking().OrderBy(k => k.Nazwa).ToList();
            return View(new Pojazd());
        }

        // POST: /Pojazdy/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pojazd model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorie = _context.KategoriePojazdow.AsNoTracking().ToList();
                return View(model);
            }
            try
            {
                _context.Pojazdy.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Pojazd dodany.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "B³¹d tworzenia pojazdu");
                ModelState.AddModelError(string.Empty, "B³¹d zapisu w bazie (unikalnoœæ numeru?).");
                ViewBag.Kategorie = _context.KategoriePojazdow.AsNoTracking().ToList();
                return View(model);
            }
        }

        // GET: /Pojazdy/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var pojazd = await _context.Pojazdy.FindAsync(id);
            if (pojazd == null) return NotFound();
            ViewBag.Kategorie = await _context.KategoriePojazdow.AsNoTracking().ToListAsync();
            return View(pojazd);
        }

        // POST: /Pojazdy/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pojazd model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorie = await _context.KategoriePojazdow.AsNoTracking().ToListAsync();
                return View(model);
            }

            try
            {
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Zapisano zmiany.";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Pojazdy.AnyAsync(p => p.Id == id)) return NotFound();
                throw;
            }
        }

        // GET: /Pojazdy/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var pojazd = await _context.Pojazdy.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (pojazd == null) return NotFound();
            return View(pojazd);
        }

        // POST: /Pojazdy/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pojazd = await _context.Pojazdy.FindAsync(id);
            if (pojazd == null) return NotFound();

            // Ewentualnie logika "soft delete" przez Status = "Wycofany"
            _context.Pojazdy.Remove(pojazd);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Pojazd usuniêty.";
            return RedirectToAction(nameof(Index));
        }
    }
}