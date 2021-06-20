using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class RecipeIngredients
	{
		public int RecipeId { get; set; }
		//[JsonIgnore]
		public Recipe Recipe { get; set; }

		public int IngredientId { get; set; }
		//[JsonIgnore]
		public Ingredient Ingredient { get; set; }

		public string Amount { get; set; }
	}
}
