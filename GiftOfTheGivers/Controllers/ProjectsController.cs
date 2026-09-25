using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProjectsController(ApplicationDbContext db) => _db = db;

        // Public: anyone can see project detail + how much has been raised so far.
        public async Task<IActionResult> Details(int id)
        {
            var project = await _db.ReliefProjects
                .Include(p => p.Donations)
                .FirstOrDefaultAsync(p => p.ProjectID == id);
            if (project == null) return NotFound();
            return View(project);
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Create() => View(new ReliefProject());

        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReliefProject project)
        {
            if (!ModelState.IsValid) return View(project);
            _db.ReliefProjects.Add(project);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _db.ReliefProjects.FindAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReliefProject project)
        {
            if (id != project.ProjectID) return BadRequest();
            if (!ModelState.IsValid) return View(project);
            _db.Update(project);
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", new { id });
        }
    }
}
