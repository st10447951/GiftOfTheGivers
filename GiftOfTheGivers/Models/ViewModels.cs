using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required, StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Donor"; // Donor or Volunteer at self-registration
    }

    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public class DonationViewModel
    {
        public int ProjectID { get; set; }
        public string? ProjectName { get; set; }

        [Required]
        public string? DonorName { get; set; } // used only for guest donations

        [Required, Range(1, 10000000)]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "ZAR";

        [Required]
        public string DonationType { get; set; } = "OneTime";
    }

    public class VolunteerRegistrationViewModel
    {
        [Required, StringLength(250)]
        public string Skills { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Availability { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string City { get; set; } = string.Empty;
    }
}
