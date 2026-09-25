using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using GiftOfTheGivers.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    // Deliberately NOT [Authorize] on the whole controller:
    // "As a guest, I want to donate anonymously without registering" (Epic 3).
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonationController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Donate(int projectId)
        {
            var project = await _db.ReliefProjects.FindAsync(projectId);
            if (project == null) return NotFound();

            return View(new DonationViewModel { ProjectID = projectId, ProjectName = project.ProjectName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Donate(DonationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var project = await _db.ReliefProjects.FindAsync(model.ProjectID);
                model.ProjectName = project?.ProjectName;
                return View(model);
            }

            var currentUser = User.Identity?.IsAuthenticated == true ? await _userManager.GetUserAsync(User) : null;

            var donation = new Donation
            {
                DonorUserID = currentUser?.Id,
                DonorName = currentUser?.FullName ?? model.DonorName,
                ProjectID = model.ProjectID,
                Amount = model.Amount,
                Currency = model.Currency,
                DonationType = model.DonationType,
                DonationDate = DateTime.UtcNow,
                TaxCertificateIssued = true // placeholder: real cert generation is a Sprint 2 item
            };

            _db.Donations.Add(donation);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Certificate), new { id = donation.DonationID });
        }

        // Placeholder tax certificate, shown on screen (Sprint 1 only stores a dummy record,
        // per the brief: "even if only a dummy record" / "displayed on screen").
        public async Task<IActionResult> Certificate(int id)
        {
            var donation = await _db.Donations.Include(d => d.Project).FirstOrDefaultAsync(d => d.DonationID == id);
            if (donation == null) return NotFound();
            return View(donation);
        }
    }
}
