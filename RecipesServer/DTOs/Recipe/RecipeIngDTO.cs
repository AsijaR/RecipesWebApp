using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs.Recipe
{
	public class RecipeIngDTO
	{
		public NewRecipeDTO Recipe { get; set; }
		public ICollection<IngredientDTO> Ingredients {get;set;}
	}
}
