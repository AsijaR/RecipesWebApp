using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class Ingredient
	{
		public int IngredientId { get; set; }

		public string Name { get; set; }
		[NotMapped]
		public string Amount { get; set; }

		public ICollection<RecipeIngredients> Recipes { get; set; }
	}
}
