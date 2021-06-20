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

            //var users = new List<AppUser>
            //{
            //    new AppUser{
            //        FirstName = "asija", LastName = "ramovic",
            //        Address = "novi pazar",City = "novi pazar",
            //        State = "serbia", Zip = "36300",Email="as@gmail.com",
            //        UserName="asa"},
            //    new AppUser{
            //        FirstName = "jasko", LastName = "ramovic",
            //        Address = "novi pazar",City = "novi pazar",
            //        State = "serbia", Zip = "36300",Email="jasko@gmail.com",
            //        UserName="jale"}
            //};
            //foreach (var user in users)
            //{
            //    user.UserName = user.UserName.ToLower();
            //    await userManager.CreateAsync(user, "Asija123.");
            //    await userManager.AddToRoleAsync(user, "Member");
            //}

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Admin123.");
            await userManager.AddToRolesAsync(admin, new[] { "Admin" });
        }
    }
}
