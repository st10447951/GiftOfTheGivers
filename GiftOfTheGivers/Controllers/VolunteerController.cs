using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using GiftOfTheGivers.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    [Authorize] // must be logged in (as a Volunteer) to register/view their own profile
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public VolunteerController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register() => View(new VolunteerRegistrationViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(VolunteerRegistrationViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var existing = await _db.Volunteers.FirstOrDefaultAsync(v => v.UserID == user.Id);
            if (existing == null)
            {
                _db.Volunteers.Add(new Volunteer
                {
                    UserID = user.Id,
                    Skills = model.Skills,
                    Availability = model.Availability,
                    City = model.City
                });
            }
            else
            {
                existing.Skills = model.Skills;
                existing.Availability = model.Availability;
                existing.City = model.City;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        // Employee dashboard: view all registered volunteers to assign them to projects.
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Index()
        {
            var volunteers = await _db.Volunteers.Include(v => v.User).ToListAsync();
            return View(volunteers);
        }
    }
}
