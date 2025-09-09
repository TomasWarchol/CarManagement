using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CarManagementApp.Models;
using CarManagementApp.Models.ViewModels;

namespace CarManagementApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly CarManagementContext _context;

        public HomeController(CarManagementContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index() => View();

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var today = DateTime.Today;
            var plus30 = today.AddDays(30);

            var aktywne = await _context.Pojazdy.CountAsync(p => p.Status == "Aktywny");
            var wygPolisy = await _context.Polisy
                .CountAsync(p => p.DataDo >= today && p.DataDo <= plus30);

            var nadchSerwisy = await _context.Serwisy
                .Where(s => s.Data >= today && s.Data <= plus30)
                .CountAsync();

            var kierowcy = await _context.Kierowcy.CountAsync();

            var polisyTerminy = await _context.Polisy
                .Where(p => p.DataDo >= today && p.DataDo <= plus30)
                .Include(p => p.Pojazd)
                .Select(p => new ZblizajacyTerminDto
                {
                    Typ = "Polisa " + p.Typ,
                    Data = p.DataDo,
                    Opis = p.Numer,
                    PojazdId = p.PojazdId,
                    NumerRejestracyjny = p.Pojazd.NumerRejestracyjny
                }).ToListAsync();

            var serwisyTerminy = await _context.Serwisy
                .Where(s => s.Data >= today && s.Data <= plus30)
                .Include(s => s.Pojazd)
                .Select(s => new ZblizajacyTerminDto
                {
                    Typ = "Serwis",
                    Data = s.Data,
                    Opis = s.Rodzaj ?? "Serwis",
                    PojazdId = s.PojazdId,
                    NumerRejestracyjny = s.Pojazd.NumerRejestracyjny
                }).ToListAsync();

            var vm = new DashboardViewModel
            {
                LiczbaAktywnychPojazdow = aktywne,
                WygasajacePolisy30Dni = wygPolisy,
                NadchodzaceSerwisy30Dni = nadchSerwisy,
                LiczbaKierowcow = kierowcy,
                ZblizajaceTerminy = polisyTerminy.Concat(serwisyTerminy)
                    .OrderBy(t => t.Data)
                    .Take(20)
                    .ToList()
            };

            return View(vm);
        }

        [AllowAnonymous]
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
