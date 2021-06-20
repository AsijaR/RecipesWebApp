using RecipesServer.DTOs.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs.Category
{
	public class CategoryDTO
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public ICollection<RecipeDTO> Recipes { get; set; }
	}
}
