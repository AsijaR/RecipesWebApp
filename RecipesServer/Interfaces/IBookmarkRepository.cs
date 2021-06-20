using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface IBookmarkRepository
	{
		bool RecipeExistInBookmark(int bookmarkId, int recipeId);
		Task<Bookmark> GetUserBookmark(int userId);
		void AddToBookmark(int bookmarkId, int recipeId);
		Task<Bookmark> DeleteFromBookmark(int bookmarkId, int recipeId);

		int GetUserBookmarkId(int userId);
	}
}
