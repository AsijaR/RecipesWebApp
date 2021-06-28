using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class AppUser : IdentityUser<int>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public float ShippingPrice { get; set; } = 2;
		public UserPhoto UserPhoto { get; set; } 
		public ICollection<AppUserRole> UserRoles { get; set; }
		public ICollection<RecipeOrders>? Orders { get; set; }
		//public ICollection<Comment>? Comments { get; set; }
		public ICollection<Recipe>? Recipes { get; set; }
	}
}
