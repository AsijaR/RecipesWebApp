using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class Bookmark
	{
		public int BookmarkId { get; set; }
		public ICollection<RecipeBookmarks>? Recipes { get; set; }
		[ForeignKey("AppUser")]
		public int UserId { get; set; }
	}
}
