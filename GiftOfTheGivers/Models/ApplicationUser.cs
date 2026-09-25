using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers.Models
{
    // Extends the built-in Identity user with the fields Users needs
    // beyond authentication (matches the Users entity in the ERD).
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    }
}
