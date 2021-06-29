using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class Comment
	{
		public int CommentId { get; set; }
		public string Message { get; set; }
		public DateTime DateCommentIsPosted { get; set; }
		public int AppUserId { get; set; }
		public AppUser User { get; set; }
		//public User User { get; set; }
		public ICollection<RecipeComments>? Recipes { get; set; }
	}
}
