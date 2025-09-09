using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using CarManagementApp.Models;

namespace CarManagementApp.Controllers
{
    [Authorize]
    public class RaportyController : Controller
    {
        private readonly CarManagementContext _context;
        public RaportyController(CarManagementContext context) => _context = context;

        // Koszt paliwa + serwisy per pojazd (prosty przyk³ad)
        public async Task<IActionResult> KosztEksploatacji()
        {
            var paliwo = await _context.Tankowania
                .GroupBy(t => t.PojazdId)
                .Select(g => new { PojazdId = g.Key, Paliwo = g.Sum(x => x.Kwota) })
                .ToListAsync();

            var serwisy = await _context.Serwisy
                .GroupBy(s => s.PojazdId)
                .Select(g => new { PojazdId = g.Key, Serwis = g.Sum(x => x.Koszt) })
                .ToListAsync();

            var pojazdy = await _context.Pojazdy.AsNoTracking().ToDictionaryAsync(p => p.Id);

            var sb = new StringBuilder();
            sb.AppendLine("NumerRejestracyjny;KosztPaliwo;KosztSerwisy;Suma");

            var allIds = paliwo.Select(p => p.PojazdId).Union(serwisy.Select(s => s.PojazdId)).Distinct();
            foreach (var id in allIds)
            {
                var p = pojazdy[id];
                var kPaliwo = paliwo.FirstOrDefault(x => x.PojazdId == id)?.Paliwo ?? 0m;
                var kSerwis = serwisy.FirstOrDefault(x => x.PojazdId == id)?.Serwis ?? 0m;
                sb.AppendLine($"{p.NumerRejestracyjny};{kPaliwo};{kSerwis};{kPaliwo + kSerwis}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "koszt_eksploatacji.csv");
        }
    }
}