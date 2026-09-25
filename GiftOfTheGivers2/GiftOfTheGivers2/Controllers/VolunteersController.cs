using GiftOfTheGivers2.Data;
using GiftOfTheGivers2.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers2.Controllers
{
    public class VolunteersController : Controller
    {
        private readonly InMemoryVolunteerStore _store;

        public VolunteersController(InMemoryVolunteerStore store)
        {
            _store = store;
        }

        // GET: /Volunteers/Register
        public IActionResult Register()
        {
            return View(new Volunteer());
        }

        // POST: /Volunteers/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Volunteer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _store.Add(model);
            return RedirectToAction(nameof(Confirmation));
        }

        // GET: /Volunteers/Confirmation
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}