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
			return _context.Recipes.Include(i=>i.Ingredients).ThenInclude(i=>i.Ingredient).SingleOrDefault(r => r.RecipeId == recipeId);
		}

		public int ingredientExists(RecipeIngredients ing)
		{
			var exists = _context.RecipeIngredients.Where(ing => ing.Ingredient.Name == ing.Ingredient.Name.ToLower()).Any();
			if (exists)
			{
				var c=_context.RecipeIngredients.FirstOrDefault(i => i.Ingredient.Name == ing.Ingredient.Name).IngredientId;
				return c;
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
				var alreadyExistIgredients = new Dictionary<int, string>();
				var doesntExistIgredients = new List<RecipeIngredients>();
			
				foreach (var i in newRecipeDTO.Ingredients)
				{

					if (ingredientExists(_mapper.Map<RecipeIngredients>(i)) is var id && id != 0)
					{
						alreadyExistIgredients.Add(id, i.Amount);
					}
					else
					{
						doesntExistIgredients.Add(_mapper.Map<RecipeIngredients>(i));
					}
				}
				if (doesntExistIgredients.Count > 0)
					_context.Ingredients.AddRange(doesntExistIgredients.Select(p => p.Ingredient));
				var r = _mapper.Map<Recipe>(newRecipeDTO.Recipe);
				_context.Recipes.Add(r);
				_context.SaveChanges();

				foreach (KeyValuePair<int, string> entry in alreadyExistIgredients)
				{
					var recIng = new RecipeIngredients
					{
						RecipeId = r.RecipeId,
						IngredientId = entry.Key,
						Amount = entry.Value
					};
					_context.Add(recIng);
					_context.SaveChanges();
				} 

				foreach (var i in doesntExistIgredients)
				{
					var recIng = new RecipeIngredients
					{
						RecipeId = r.RecipeId,
						IngredientId = i.Ingredient.IngredientId,
						Amount = i.Amount
					};
					_context.Add(recIng);
					_context.SaveChanges();
				}
			}
			return newRecipeDTO;
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<RecipeUpdateDTO> UpdateAsync(Recipe recipe, RecipeUpdateDTO recipeDTO)
		{
			//var recipeInDb = _context.Recipes.SingleOrDefault(r=>r.RecipeId==recipe.re);
			var novi = _mapper.Map<Recipe>(recipeDTO);
			var ff = recipe.Ingredients;
			//var allCurrentIngredients = _context.RecipeIngredients.Where(r => r.RecipeId == recipe.RecipeId).ToList();
			var alreadyExistIgredients = new Dictionary<int, string>();
			var doesntExistIgredients = new List<Ingredient>();
			var r = new List<RecipeIngredients>();
			var d = new List<RecipeIngredients>();

			foreach (var ing in recipe.Ingredients)
			{
				var c= novi.Ingredients.Any(i=>i.Ingredient.Name==ing.Ingredient.Name);
				if (!c)
					r.Add(ing);
				else
					d.Add(ing);
			}

			foreach (var i in novi.Ingredients)
			{
				if (ingredientExists(i) is var id && id != 0 )
				{
					alreadyExistIgredients.Add(id, i.Amount);
				}
				else
				{
					doesntExistIgredients.Add(i.Ingredient);
					//_context.Ingredients.AddAsync(i.Ingredient);
				}
			}

			if(r.Count()>0)
				 r.ForEach(x => recipe.Ingredients.Remove(x));

			if (doesntExistIgredients.Count > 0) 
			{
				 _context.Ingredients.AddRangeAsync(doesntExistIgredients);
				 _context.SaveChanges();
				_context.RecipeIngredients.AsNoTracking();
			}
				


			foreach (KeyValuePair<int, string> entry in alreadyExistIgredients)
			{
				if (d.Any(i=>i.IngredientId!=entry.Key)) {
				var recIng = new RecipeIngredients
				{
					RecipeId = recipe.RecipeId,
					IngredientId = entry.Key,
					Amount = entry.Value
				};
				 _context.RecipeIngredients.AddAsync(recIng);
				}
			}
			foreach (var i in doesntExistIgredients)
			{
				//var e =  _context.Ingredients.SingleOrDefaultAsync(i=>i.Name==i.Name);
				var recIng = new RecipeIngredients
				{
					RecipeId = recipe.RecipeId,
					IngredientId =i.IngredientId,
					Amount = i.Amount
				};
				_context.RecipeIngredients.AddAsync(recIng);
			}
			
			_context.Entry(recipe).State = EntityState.Modified;
			_context.SaveChanges();
			return recipeDTO;
		}
	}
}
