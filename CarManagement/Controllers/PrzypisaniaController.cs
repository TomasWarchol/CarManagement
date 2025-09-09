using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarManagementApp.Models;
using CarManagementApp.Models.ViewModels;

namespace CarManagementApp.Controllers
{
    [Authorize]
    public class PrzypisaniaController : Controller
    {
        private readonly CarManagementContext _context;

        public PrzypisaniaController(CarManagementContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var aktywne = await _context.Przypisania
                .Include(p => p.Pojazd)
                .Include(p => p.Kierowca)
                .Where(p => p.DataOd <= today && (p.DataDo == null || p.DataDo >= today))
                .AsNoTracking()
                .OrderBy(p => p.Pojazd.NumerRejestracyjny)
                .ToListAsync();
            return View(aktywne);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new PrzypisanieCreateViewModel
            {
                PojazdySelect = await _context.Pojazdy.AsNoTracking().OrderBy(p => p.NumerRejestracyjny).ToListAsync(),
                KierowcySelect = await _context.Kierowcy.AsNoTracking().OrderBy(k => k.Nazwisko).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrzypisanieCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.PojazdySelect = await _context.Pojazdy.AsNoTracking().ToListAsync();
                vm.KierowcySelect = await _context.Kierowcy.AsNoTracking().ToListAsync();
                return View(vm);
            }

            // Zakoñcz poprzednie aktywne przypisanie tego pojazdu (jeœli brak DataDo)
            var aktywnePojazdu = await _context.Przypisania
                .Where(p => p.PojazdId == vm.PojazdId && p.DataDo == null)
                .ToListAsync();
            foreach (var a in aktywnePojazdu)
                a.DataDo = vm.DataOd.AddDays(-1);

            var nowe = new PrzypisaniePojazdu
            {
                PojazdId = vm.PojazdId,
                KierowcaId = vm.KierowcaId,
                DataOd = vm.DataOd,
                DataDo = vm.DataDo,
                TrybUzytkowania = vm.TrybUzytkowania
            };
            _context.Przypisania.Add(nowe);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Dodano przypisanie.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Zakoncz(int id)
        {
            var przyp = await _context.Przypisania.FindAsync(id);
            if (przyp == null) return NotFound();
            if (przyp.DataDo == null)
            {
                przyp.DataDo = DateTime.Today;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Przypisanie zakoñczone.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}