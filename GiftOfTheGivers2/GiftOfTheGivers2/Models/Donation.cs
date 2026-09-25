using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers2.Models
{
    public class Donation
    {
        public int DonationId { get; set; }

        [Display(Name = "Donor Name")]
        [StringLength(100)]
        public string? DonorName { get; set; }

        // Set server-side from the logged-in user; not bound from the form
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Please enter a donation amount.")]
        [Range(1, 1000000, ErrorMessage = "Amount must be between 1 and 1,000,000.")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Currency")]
        public string Currency { get; set; } = "ZAR";

        [Required]
        [Display(Name = "Donation Type")]
        public string DonationType { get; set; } = "OneTime";

        [Display(Name = "Donate Anonymously")]
        public bool IsAnonymous { get; set; }

        public DateTime DonationDate { get; set; } = DateTime.Now;

        public string? CertificateNumber { get; set; }
    }
}