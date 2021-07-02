using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class RecipeOrders
	{
		public int OrderId { get; set; }
		public Order Order { get; set; }

		[ForeignKey("User")]
		public int? UserId { get; set; }

		[ForeignKey("User")]
		public int? ChefId { get; set; }
		public AppUser Chef { get; set; }

		public int? RecipeId { get; set; }
		public Recipe Recipe { get; set; }

		public string ApprovalStatus { get; set; } = "Waiting";
	}
}
