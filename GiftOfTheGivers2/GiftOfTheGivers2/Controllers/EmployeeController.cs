using GiftOfTheGivers2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers2.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly InMemoryVolunteerStore _volunteerStore;
        private readonly InMemoryDonationStore _donationStore;

        public EmployeeController(InMemoryVolunteerStore volunteerStore, InMemoryDonationStore donationStore)
        {
            _volunteerStore = volunteerStore;
            _donationStore = donationStore;
        }

        // GET: /Employee/Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.Volunteers = _volunteerStore.GetAll();
            ViewBag.Donations = _donationStore.GetAll();
            return View();
        }
    }
}