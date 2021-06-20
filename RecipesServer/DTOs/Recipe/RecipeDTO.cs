using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs.Recipe
{
	public class RecipeDTO
	{
		public int RecipeId { get; set; }
		public string ChefName { get; set; }
		public string Title { get; set; }
		public string PhotoUrl { get; set; }
		public string Complexity { get; set; }
		public int ServingNumber { get; set; }
		public string TimeNeededToPrepare { get; set; }
		public string Description { get; set; }
		public string Note { get; set; }
		public bool MealCanBeOrdered { get; set; }
		public float Price { get; set; }
		public int MaxServingNumber { get; set; }
		public string NoteForShipping { get; set; }
		public int CategoryId { get; set; }
		public ICollection<IngredientDTO> Ingredients { get; set; }
		public ICollection<CommentDTO> Comments { get; set; }
		//public Comment[] Comments { get; set; }
		//ovo mi treba dto
		//public ICollection<RecipeIngredients> Ingredients { get; set; }
	}
}
