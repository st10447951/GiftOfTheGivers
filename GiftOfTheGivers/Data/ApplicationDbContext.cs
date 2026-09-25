using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Data
{
    // ApplicationUser already carries the Role via AspNetRoles / UserRoles,
    // so IdentityDbContext gives us Users + roles for free, and we add
    // the four extra entities from the ERD on top.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ReliefProject> ReliefProjects { get; set; } = null!;
        public DbSet<Volunteer> Volunteers { get; set; } = null!;
        public DbSet<VolunteerAssignment> VolunteerAssignments { get; set; } = null!;
        public DbSet<Donation> Donations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Indexes that match the optimisation strategy in Section 2.3 of the report
            builder.Entity<Donation>()
                .HasIndex(d => new { d.ProjectID, d.DonationDate });

            builder.Entity<VolunteerAssignment>()
                .HasIndex(a => a.ProjectID);

            builder.Entity<Donation>()
                .Property(d => d.Amount)
                .HasPrecision(18, 2);

            builder.Entity<ReliefProject>()
                .Property(p => p.TargetAmount)
                .HasPrecision(18, 2);

            // A donor is optional (guest donations); if a user is deleted,
            // keep the donation record rather than cascading the delete.
            builder.Entity<Donation>()
                .HasOne(d => d.DonorUser)
                .WithMany()
                .HasForeignKey(d => d.DonorUserID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
