using GiftOfTheGivers2.Data;
using GiftOfTheGivers2.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers2.Controllers
{
    public class DonationsController : Controller
    {
        private readonly InMemoryDonationStore _store;

        public DonationsController(InMemoryDonationStore store)
        {
            _store = store;
        }

        // GET: /Donations
        public IActionResult Index()
        {
            return View(new Donation());
        }

        // POST: /Donations
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Donation model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.IsAnonymous)
            {
                model.UserId = null;
                if (string.IsNullOrWhiteSpace(model.DonorName))
                {
                    model.DonorName = "Anonymous Donor";
                }
            }
            else
            {
                model.UserId = User.Identity?.IsAuthenticated == true ? User.Identity!.Name : null;
                if (string.IsNullOrWhiteSpace(model.DonorName) && User.Identity?.IsAuthenticated == true)
                {
                    model.DonorName = User.Identity!.Name;
                }
            }

            var saved = _store.Add(model);
            return RedirectToAction(nameof(Confirmation), new { id = saved.DonationId });
        }

        // GET: /Donations/Confirmation/5
        public IActionResult Confirmation(int id)
        {
            var donation = _store.GetById(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }
    }
}