
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymSystem.DAL.DataSeeds
{
    public static class IdentityDataSeeds
    {
        public static async Task SeedIdentityAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger logger,
            CancellationToken ct = default)
        {
            try
            {
                bool HasUsers = await userManager.Users.AnyAsync(ct);
                bool HasRoles = await roleManager.Roles.AnyAsync(ct);

                if (HasUsers && HasRoles)
                {
                    logger.LogInformation("Identity data already exists. Skipping seeding.");
                    return;
                }

                if (!HasRoles)
                {
                    var roles = new List<IdentityRole>()
                    {
                        new IdentityRole("SuperAdmin"),
                        new IdentityRole("Admin"),
                    }; 

                    foreach (var roleName in roles.Select(r => r.Name))
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            var identityRole = new IdentityRole(roleName);
                            var result = await roleManager.CreateAsync(identityRole);
                            if (result.Succeeded)
                            {
                                logger.LogInformation($"Role '{roleName}' created successfully.");
                            }
                            else
                            {
                                logger.LogError($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            }
                        }
                    }
                }

                if (!HasUsers)
                {
                    var User = new ApplicationUser
                    {
                        FirstName = "Mohamed",
                        LastName = "Omara",
                        UserName = "MohamedOmara",
                        Email = "mohamed.omara@gmail.com",
                        PhoneNumber = "01001025572",
                    };

                    var result = await userManager.CreateAsync(User,"MohamedOmara@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(User, "Admin");
                    }
                    else
                    {
                        logger.LogError($"Failed to create user '{User.UserName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while seeding identity data");
                throw;
            }
        }
    }
}
