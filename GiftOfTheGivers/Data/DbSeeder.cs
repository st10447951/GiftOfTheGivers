using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers.Data
{
    // Called once at startup to make sure the Donor/Volunteer/Employee
    // roles exist and there is at least one sample project to donate to.
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var db = services.GetRequiredService<ApplicationDbContext>();

            foreach (var role in new[] { "Donor", "Volunteer", "Employee" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Sample employee login so the dashboard can be demoed immediately.
            // CHANGE THIS PASSWORD before deploying anywhere public.
            const string employeeEmail = "employee@giftofthegivers.local";
            if (await userManager.FindByEmailAsync(employeeEmail) is null)
            {
                var employee = new ApplicationUser
                {
                    UserName = employeeEmail,
                    Email = employeeEmail,
                    FullName = "Demo Employee",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(employee, "Employee@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(employee, "Employee");
            }

            if (!db.ReliefProjects.Any())
            {
                db.ReliefProjects.Add(new ReliefProject
                {
                    ProjectName = "Flood Relief \u2013 KwaZulu-Natal",
                    Description = "Emergency food, water, and shelter for households displaced by flooding.",
                    Location = "KwaZulu-Natal, South Africa",
                    StartDate = DateTime.UtcNow,
                    Status = "Active",
                    TargetAmount = 500000m
                });
                db.ReliefProjects.Add(new ReliefProject
                {
                    ProjectName = "Drought Response \u2013 Eastern Cape",
                    Description = "Water tankering and borehole repair for rural communities.",
                    Location = "Eastern Cape, South Africa",
                    StartDate = DateTime.UtcNow,
                    Status = "Active",
                    TargetAmount = 350000m
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
