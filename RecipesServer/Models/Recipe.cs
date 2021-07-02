using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class Recipe
	{
		public int RecipeId { get; set; }
		public string Title { get; set; }
		public string Complexity { get; set; }
		public int ServingNumber { get; set; }
		public string TimeNeededToPrepare { get; set; }
		public string Description { get; set; }
		public string Note { get; set; }
		public bool MealCanBeOrdered { get; set; } = false;
		public float Price { get; set; }
		public string NoteForShipping { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		[ForeignKey("Category")]
		public int? CategoryId { get; set; }
		//[ForeignKey("AppUser")]
		public int AppUserId { get; set; }
		public AppUser User { get; set; }

		public ICollection<RecipeIngredients> Ingredients { get; set; }
		public ICollection<RecipeOrders>? RecipeOrders { get; set; }
		public ICollection<RecipeBookmarks>? Bookmarks { get; set; }
		public ICollection<RecipeComments>? Comments { get; set; }
		public ICollection<RecipePhotos> RecipePhotos { get; set; }
	}
}
