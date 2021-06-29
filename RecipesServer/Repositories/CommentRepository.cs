using AutoMapper;
using RecipesServer.Data;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipesServer.DTOs.Comment;

namespace RecipesServer.Repositories
{
	public class CommentRepository : ICommentRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public CommentRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async void AddComment(int recipeId, int userId, AddCommentDTO comment)
		{
				var commentToAdd = new Comment { AppUserId = userId, DateCommentIsPosted = DateTime.Now, Message = comment.Message };
				await _context.RecipeCommments.AddAsync(new RecipeComments { RecipeId = recipeId, Comment= commentToAdd });
		}

		public void DeleteComment(int recipeId, int commentId)
		{
			 _context.RecipeCommments.Remove(new RecipeComments { RecipeId = recipeId, CommentId = commentId });
		}

		public async Task<Comment> GetCommentAsync(int commentId)
		{
			return _context.Comments.SingleOrDefault(c => c.CommentId == commentId);
		}
		public async void DeleteCommentAsync(Comment comment)
		{
			 _context.Comments.Remove(comment);
		}

	}
}
