using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class ReliefProject
    {
        public int ProjectID { get; set; }

        [Required, StringLength(150)]
        public string ProjectName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [StringLength(150)]
        public string? Location { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }

        [StringLength(30)]
        public string Status { get; set; } = "Active";

        [Column(TypeName = "decimal(18,2)")]
        public decimal TargetAmount { get; set; }

        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
        public ICollection<VolunteerAssignment> Assignments { get; set; } = new List<VolunteerAssignment>();
    }

    public class Volunteer
    {
        public int VolunteerID { get; set; }

        [Required]
        public string UserID { get; set; } = string.Empty; // FK -> AspNetUsers
        public ApplicationUser? User { get; set; }

        [StringLength(250)]
        public string? Skills { get; set; }

        [StringLength(100)]
        public string? Availability { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        public ICollection<VolunteerAssignment> Assignments { get; set; } = new List<VolunteerAssignment>();
    }

    public class VolunteerAssignment
    {
        public int AssignmentID { get; set; }

        public int VolunteerID { get; set; }
        public Volunteer? Volunteer { get; set; }

        public int ProjectID { get; set; }
        public ReliefProject? Project { get; set; }

        [StringLength(100)]
        public string? RoleOnProject { get; set; }

        public DateTime DateAssigned { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(6,2)")]
        public decimal HoursLogged { get; set; }
    }

    public class Donation
    {
        public int DonationID { get; set; }

        // Nullable so guests can donate anonymously (Users -> Donations is 1 to 0..many)
        public string? DonorUserID { get; set; }
        public ApplicationUser? DonorUser { get; set; }

        public string? DonorName { get; set; } // captured for guest donations

        public int ProjectID { get; set; }
        public ReliefProject? Project { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(1, 10000000)]
        public decimal Amount { get; set; }

        [Required, StringLength(3)]
        public string Currency { get; set; } = "ZAR"; // ZAR / USD / EUR

        [Required, StringLength(20)]
        public string DonationType { get; set; } = "OneTime"; // OneTime / Recurring

        public DateTime DonationDate { get; set; } = DateTime.UtcNow;

        public bool TaxCertificateIssued { get; set; } = false;
    }
}
