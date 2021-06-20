using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class RecipeComments
	{
		public int RecipeId { get; set; }
		public Recipe Recipe { get; set; }

		public int CommentId { get; set; }
		public Comment Comment { get; set; }
	}
}
