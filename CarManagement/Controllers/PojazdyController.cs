using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarManagement.Models;

namespace CarManagement.Controllers
{
    [Authorize] // Dostêp tylko dla zalogowanych u¿ytkowników
    public class PojazdyController : Controller
    {
        private readonly CarManagementContext _context;

        public PojazdyController(CarManagementContext context)
        {
            _context = context;
        }

        // GET: Pojazdy
        public async Task<IActionResult> Index()
        {
            var pojazdy = await _context.Pojazdy
                .Include(p => p.Kategoria)
                .AsNoTracking()
                .ToListAsync();
                
            return View(pojazdy);
        }
    }
}