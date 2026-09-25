using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers2.Models
{
    public class Volunteer
    {
        public int VolunteerId { get; set; }

        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a contact email.")]
        [EmailAddress]
        [Display(Name = "Contact Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please list at least one skill.")]
        [StringLength(255)]
        public string Skills { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please indicate your availability.")]
        [Display(Name = "Availability")]
        public string Availability { get; set; } = "Weekends";

        public DateTime RegisteredDate { get; set; } = DateTime.Now;
    }
}