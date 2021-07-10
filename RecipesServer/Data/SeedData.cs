using Microsoft.AspNetCore.Identity;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Data
{
	public class SeedData
	{
        public static async Task SeedUsers(UserManager<AppUser> userManager,
           RoleManager<AppRole> roleManager)
        {
            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin", FirstName = "Asija", LastName = "Ramovic", Email = "asija.np@gmail.com", EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin123.");
            await userManager.AddToRolesAsync(admin, new[] { "Admin" });
        }
    }
}
