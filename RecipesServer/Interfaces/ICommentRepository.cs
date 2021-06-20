using RecipesServer.DTOs;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface ICommentRepository
	{
		void AddComment(int recipeId, int userId, AddCommentDTO comment);
		void DeleteComment(int recipeId, int commentId);
		Task<Comment> GetCommentAsync(int commentId);
		void DeleteCommentAsync(Comment comment);
	}
}
