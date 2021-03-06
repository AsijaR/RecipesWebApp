using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs.Recipe
{
	public class NewRecipeDTO
	{
		public int RecipeId { get; set; }
		public int UserId { get; set; }
		public string Title { get; set; }
		public string Complexity { get; set; }
		public int ServingNumber { get; set; }
		public string TimeNeededToPrepare { get; set; }
		public string Description { get; set; }
		public string Note { get; set; }
		public bool MealCanBeOrdered { get; set; }
		public float Price { get; set; }
		public string NoteForShipping { get; set; }
		public int CategoryId { get; set; }
		//public IFormFile file { get; set; }
		//public RecipePhotoDTO RecipePhotos { get; set; }
		//public ICollection<IngredientDTO> Ingredients { get; set; }
	}
}
