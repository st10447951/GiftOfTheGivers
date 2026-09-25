using GiftOfTheGivers.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var projects = await _db.ReliefProjects
                .Where(p => p.Status == "Active")
                .OrderByDescending(p => p.StartDate)
                .ToListAsync();
            return View(projects);
        }

        public IActionResult About() => View();

        public IActionResult Error() => View();
    }
}
