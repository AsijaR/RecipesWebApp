using RecipesServer.DTOs.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs
{
	public class BookmarkDTO
	{
		public int BookmarkId { get; set; }
		public ICollection<RecipeBasicInfoDTO> Recipes { get; set; }
	}
}
