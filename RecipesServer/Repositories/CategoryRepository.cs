using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RecipesServer.Data;
using RecipesServer.DTOs;
using RecipesServer.DTOs.Category;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public CategoryRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public void AddCategory(CategoryUpdateDTO category)
		{
			var cat = _mapper.Map<Category>(category);
			_context.Categories.AddAsync(cat);
		}

		public async Task<IEnumerable<AllCategoriesDTO>> GetAllCategories()
		{
			 var cat= _context.Categories.ProjectTo<AllCategoriesDTO>(_mapper.ConfigurationProvider).ToList();;
			return cat;
		}

		public async Task<CategoryDTO> GetCategory(int categoryId)
		{
			return await _context.Categories.Include(r => r.Recipes)
											.ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
											.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
		}

		public void UpdateCategory(AllCategoriesDTO category)
		{
			var p=_mapper.Map<Category>(category);
			var cat =  _context.Categories.Find(p.CategoryId);
			//var c = _mapper.Map<Category>(cat)
			if (cat != null) 
			{ 
				_context.Entry(cat).State = EntityState.Modified;
			}
		
		}

		public async Task<Category> AddRecipeToCategory(int categoryId, int recipeId)
		{
			var findCategory = await _context.Categories.FindAsync(categoryId);
			var recipe = await _context.Recipes.SingleOrDefaultAsync(b => b.RecipeId == recipeId);
			if (findCategory != null)
			{
				findCategory.Recipes.Add(recipe);
				_context.Entry(findCategory).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			return null;
			/*var findUserBookmark = await _context.Bookmarks.FindAsync(bookmarkId);
			var recipe = await _context.Recipes.SingleOrDefaultAsync(b => b.RecipeId == recipeId);
			recipe.ExistInBookmark = true;
			if (findUserBookmark != null)
			{
				_context.RecipeBookmarks.Add(new RecipeBookmarks { RecipeId = recipeId, BookmarkId = bookmarkId}); ;
				_context.SaveChanges();
			}
			return null;*/
		}
		public async Task<Category> RemoveRecipeFromCategory(int categoryId, int recipeId)
		{
			var findCategory = await _context.Categories.FindAsync(categoryId);
			var recipe = findCategory.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
			if (findCategory.Recipes.Contains(recipe))
			{
				//findCategory.Recipes.Remove(recipe);
				System.Console.WriteLine("nalazi se");
				//var uncategoriezed=await _context.Categories.FindAsync(5);
				//uncategoriezed.Recipes.Add(recipe);
				//_context.Entry(findCategory).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			return null;
		}

		public async Task<AllCategoriesDTO> DeleteCategory(int categoryId)
		{
			var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
			if (category != null)
			{
				_context.Categories.Remove(category);
				return _mapper.Map<AllCategoriesDTO>(category);
			}
			else return null;
		}

	}
}
