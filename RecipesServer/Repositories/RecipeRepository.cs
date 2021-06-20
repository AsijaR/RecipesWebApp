using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RecipesServer.Data;
using RecipesServer.DTOs.Recipe;
using RecipesServer.Helpers;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Repositories
{
	public class RecipeRepository : IRecipeRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public RecipeRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<Recipe> GetRecipeAsync(int recipeId)
		{
			var recipe = await _context.Recipes.Include(ing => ing.Ingredients)
												.ThenInclude(i => i.Ingredient)
												.Include(c => c.Comments)
												.ThenInclude(com => com.Comment)
												.SingleOrDefaultAsync(r => r.RecipeId == recipeId);

			return recipe;
		}

		public int ingredientExists(Ingredient ingredient)
		{
			var exists = _context.Ingredients.Where(ing => ing.Name == ingredient.Name.ToLower()).Any();
			if (exists)
			{
				return _context.Ingredients.FirstOrDefault(i => i.Name == ingredient.Name).IngredientId;
			}
			else
			{
				return 0;
			}
		}

		public async Task<RecipeDTO> GetRecipeByIdAsync(int id)
		{
			var recipe = await _context.Recipes.Include(c=>c.Ingredients)
												.Include(c=>c.Comments)
												.ProjectTo<RecipeDTO>(_mapper.ConfigurationProvider)
												.SingleOrDefaultAsync(r=>r.RecipeId==id);

			RecipeDTO recipeDTO = _mapper.Map<RecipeDTO>(recipe);

			return recipeDTO;
		}

		public async Task<IEnumerable<Recipe>> GetRecipesAsync()
		{
			return await _context.Recipes.ToListAsync();
		}

		public Task<PagedList<RecipeDTO>> GetRecipesAsync(RecipeParams recipeParams)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public void Update(Recipe recipe)
		{
			_context.Entry(recipe).State = EntityState.Modified;
		}
	}
}
