using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class RecipeBookmarks
	{
		public int RecipeId { get; set; }
		public Recipe Recipe { get; set; }

		public int BookmarkId { get; set; }
		public Bookmark Bookmark { get; set; }
	}
}
