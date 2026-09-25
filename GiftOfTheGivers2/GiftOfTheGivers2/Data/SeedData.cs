using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers2.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. Create roles if they don't exist
            string[] roleNames = { "Employee", "Donor" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Seed a default Employee account for testing/demo purposes
            const string employeeEmail = "employee@giftofthegivers.org";
            const string employeePassword = "Employee@123";

            var existingEmployee = await userManager.FindByEmailAsync(employeeEmail);
            if (existingEmployee == null)
            {
                var employeeUser = new IdentityUser
                {
                    UserName = employeeEmail,
                    Email = employeeEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(employeeUser, employeePassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employeeUser, "Employee");
                }
            }
        }
    }
}