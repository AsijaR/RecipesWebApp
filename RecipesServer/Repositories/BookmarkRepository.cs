using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipesServer.Data;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipesServer.DTOs;
using AutoMapper.QueryableExtensions;

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
		public async Task<bool> RecipeExistInBookmark(int bookmarkId, int recipeId)
		{
			var exists = await _context.RecipeBookmarks.AsNoTracking().SingleOrDefaultAsync(b=>b.BookmarkId==bookmarkId && b.RecipeId==recipeId);
			if (exists != null)
				return true;
			else
				return false;
		}

		public async Task<Recipe> AddToBookmark(int bookmarkId, int recipeId)
		{
			var recipe = await _context.Recipes.SingleOrDefaultAsync(r=>r.RecipeId==recipeId);
			if (recipe != null) { 
					_context.RecipeBookmarks.Add(new RecipeBookmarks { RecipeId = recipeId, BookmarkId = bookmarkId }); ;
				return recipe;		
			}
			
			return null;
		}

		public async void DeleteFromBookmark(int bookmarkId, int recipeId)
		{
			 _context.RecipeBookmarks.Remove(new RecipeBookmarks { RecipeId = recipeId, BookmarkId = bookmarkId }); ;
		}

		public int GetUserBookmarkId(int userId)
		{
			Bookmark p=  _context.Bookmarks.SingleOrDefault(u=>u.UserId==userId);
			return p.BookmarkId;
		}

		public async Task<BookmarkDTO> GetUserBookmark(int bookmarkId)
		{
			var bookmarsRecipes =  await _context.Bookmarks.Include(br => br.Recipes)
															.ProjectTo<BookmarkDTO>(_mapper.ConfigurationProvider)
															.FirstOrDefaultAsync(b => b.BookmarkId == bookmarkId);
			return bookmarsRecipes;
		}

		
	}
}
