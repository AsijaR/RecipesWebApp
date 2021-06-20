using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs.Comment
{
	public class CommentDTO
	{
		public string FullName { get; set; }
		public string Message { get; set; }
		public DateTime DateCommentIsPosted { get; set; }
		//streba i slika osobe da se posalje
		
	}
}
