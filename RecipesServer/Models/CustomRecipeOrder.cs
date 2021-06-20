using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class CustomRecipeOrder
	{
		public int OrderId { get; set; }
		public string Title { get; set; }
		public float Price { get; set; }
		public Order Order { get; set; }
		public string ApprovalStatus { get; set; }
	}
}
