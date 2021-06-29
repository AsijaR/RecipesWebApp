using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs.Recipe
{
	public class RecipeBasicInfoDTO
	{
		public int RecipeId { get; set; }
		public string HeaderUrl { get; set; }
		public string Title { get; set; }
		public string Complexity { get; set; }
		public int ServingNumber { get; set; }
		public string TimeNeededToPrepare { get; set; }
	}
}
