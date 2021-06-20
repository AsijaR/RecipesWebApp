using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipesServer.Data;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Repositories
{
	public class BookmarkRepository : IBookmarkRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public BookmarkRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async void AddToBookmark(int bookmarkId, int recipeId)
		{
			//var findUserBookmark = await _context.Bookmarks.FindAsync(bookmarkId);
			var recipe = await _context.Recipes.SingleOrDefaultAsync(r=>r.RecipeId==recipeId);
			//if (!RecipeExistInBookmark())
			//{
				var p =_context.RecipeBookmarks.Add(new RecipeBookmarks { RecipeId = recipeId, BookmarkId = bookmarkId }); ;
				_context.SaveChangesAsync();
			//}
			//return null;
		}

		public Task<Bookmark> DeleteFromBookmark(int bookmarkId, int recipeId)
		{
			throw new NotImplementedException();
		}
		public int GetUserBookmarkId(int userId)
		{
			Bookmark p=  _context.Bookmarks.SingleOrDefault(u=>u.UserId==userId);
			return p.BookmarkId;
		}
		public Task<Bookmark> GetUserBookmark(int userId)
		{
			throw new NotImplementedException();
		}

		public bool RecipeExistInBookmark(int bookmarkId, int recipeId)
		{
			throw new NotImplementedException();
		}
	}
}
