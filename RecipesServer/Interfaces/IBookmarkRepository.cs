using RecipesServer.DTOs;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface IBookmarkRepository
	{
		Task<bool> RecipeExistInBookmark(int bookmarkId, int recipeId);
		Task<BookmarkDTO> GetUserBookmark(int bookmarkId);
		Task<Recipe> AddToBookmark(int bookmarkId, int recipeId);
		void DeleteFromBookmark(int bookmarkId, int recipeId);

		int  GetUserBookmarkId(int userId);
	}
}
