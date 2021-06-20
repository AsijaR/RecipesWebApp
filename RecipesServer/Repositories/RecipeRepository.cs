using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RecipesServer.Data;
using RecipesServer.DTOs;
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


		public async Task<RecipeDTO> GetRecipeByIdAsync(int id)
		{
			var recipe = await _context.Recipes.Include(c=>c.Ingredients)
												.Include(c=>c.Comments)
												.ProjectTo<RecipeDTO>(_mapper.ConfigurationProvider)
												.SingleOrDefaultAsync(r=>r.RecipeId==id);

			RecipeDTO recipeDTO = _mapper.Map<RecipeDTO>(recipe);

			return recipeDTO;
		}
		public async Task<IEnumerable<RecipeBasicInfoDTO>> GetUserRecipes(int userId) 
		{
			return await _context.Recipes.Where(u => u.UserId == userId)
									.ProjectTo<RecipeBasicInfoDTO>(_mapper.ConfigurationProvider)
									.ToListAsync();
		}
		public async void DeleteRecipe(Recipe recipe)
		{
			_context.Recipes.Remove(recipe);
			_context.SaveChanges();
		}

		public Task<PagedList<RecipeDTO>> GetRecipesAsync(RecipeParams recipeParams)
		{
			throw new NotImplementedException();
		}

		public async Task<Recipe> FindRecipeByIdAsync(int recipeId) 
		{ 
			return _context.Recipes.SingleOrDefault(r => r.RecipeId == recipeId);
		}

		public int ingredientExists(IngredientDTO ingredient)
		{
			var exists = _context.Ingredients.Where(ing => ing.Name == ingredient.Name.ToLower()).ProjectTo<Ingredient>(_mapper.ConfigurationProvider).Any();
			if (exists)
			{
				return _context.Ingredients.FirstOrDefault(i => i.Name == ingredient.Name).IngredientId;
			}
			else
			{
				return 0;
			}
		}

		public async Task<RecipeIngDTO> AddNewRecipe(RecipeIngDTO newRecipeDTO)
		{
			if (newRecipeDTO != null)
			{
				//var recIng = _mapper.Map<RecipeIngredients>(newRecipeDTO);
			//	var ingredients = _mapper.Map<Ingredient>(newRecipeDTO.Ingredients);
				var alreadyExistIgredients = new Dictionary<int, string>();
				var doesntExistIgredients = new List<IngredientDTO>();
				/*foreach (var i in newRecipeDTO.Ingredient)
				{
					if (ingredientExists(i) is var id && id != 0)
					{
						alreadyExistIgredients.Add(id, i.Amount);
					}
					else
					{
						doesntExistIgredients.Add(i);
					}
				}*/
				if (doesntExistIgredients.Count > 0)
					_context.Ingredients.AddRange(_mapper.Map<Ingredient>(doesntExistIgredients));
				var r = _mapper.Map<Recipe>(newRecipeDTO.Recipe);
				_context.Recipes.Add(r);
				//_context.SaveChanges();

				//foreach (KeyValuePair<int, string> entry in alreadyExistIgredients)
				//{
				//	var recIng = new RecipeIngredients
				//	{
				//		RecipeId = recipe.RecipeId,
				//		IngredientId = entry.Key,
				//		Amount = entry.Value
				//	};
				//	_context.Add(recIng);
				//	_context.SaveChanges();
				//}
				//foreach (Ingredient i in doesntExistIgredients)
				//{
				//	var recIng = new RecipeIngredients
				//	{
				//		RecipeId = recipe.RecipeId,
				//		IngredientId = i.IngredientId,
				//		Amount = i.Amount
				//	};
				//	_context.Add(recIng);
				//	_context.SaveChanges();
				//}
			}
			return newRecipeDTO;
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
