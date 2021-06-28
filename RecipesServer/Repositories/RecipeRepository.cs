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
			return _context.Recipes.Include(i=>i.Ingredients)
									.ThenInclude(i=>i.Ingredient)
									.Include(p=>p.RecipePhotos)
									.SingleOrDefault(r => r.RecipeId == recipeId);
		}

		public int ingredientExists(RecipeIngredients ingredient)
		{
			var ing = _context.Ingredients.Where(ing => ing.Name.ToLower() == ingredient.Ingredient.Name.ToLower());
			var exists = ing.Any();
			if (exists)
			{
				var c = ing.FirstOrDefault().IngredientId;
				return c;
			}
			else
			{
				return 0;
			}
		}

		public async Task<Int32> AddNewRecipe(RecipeIngDTO newRecipeDTO)
		{
			var r = _mapper.Map<Recipe>(newRecipeDTO.Recipe);
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
				_context.Entry(r).GetDatabaseValues();

			}
			return r.RecipeId;
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<RecipeUpdateDTO> UpdateRecipe(Recipe recipe,RecipeUpdateDTO recipeDTO)
		{
			var recipeInDb = recipe;
			var newRecipe = _mapper.Map<Recipe>(recipeDTO);
			var alreadyExistIgredients = new Dictionary<int, string>();
			var doesntExistIgredients = new Dictionary<Ingredient, string>(); ;
			var ingredientsToRemove = new List<RecipeIngredients>();
			var ingredientsWhoStays = new List<RecipeIngredients>();
			//ovo su mi original ingredients od recepta
			foreach (var ing in recipeInDb.Ingredients)
			{
				var c= newRecipe.Ingredients.Any(i=>i.Ingredient.Name==ing.Ingredient.Name);
				if (!c)
					ingredientsToRemove.Add(ing);
				else
					ingredientsWhoStays.Add(ing);
			}

			foreach (var i in newRecipe.Ingredients)
			{
				if (ingredientExists(i) is var id && id != 0 )
				{
					alreadyExistIgredients.Add(id, i.Amount);
				}
				else
				{
					doesntExistIgredients.Add(i.Ingredient,i.Amount);
				}
			}

			if(ingredientsToRemove.Count()>0)
				ingredientsToRemove.ForEach(x => recipeInDb.Ingredients.Remove(x));

			if (doesntExistIgredients.Count > 0) 
			{
				 await _context.Ingredients.AddRangeAsync(doesntExistIgredients.Keys);
				 _context.SaveChanges();
				_context.RecipeIngredients.AsNoTracking();
			}
				
			foreach (KeyValuePair<int, string> entry in alreadyExistIgredients)
			{
				var ing = recipeInDb.Ingredients.FirstOrDefault(ing => ing.IngredientId == entry.Key);
				if (ing == null) 
				{
					var recIng = new RecipeIngredients
					{
						RecipeId = recipeInDb.RecipeId,
						IngredientId = entry.Key,
						Amount = entry.Value
					};
					await _context.RecipeIngredients.AddAsync(recIng);
				}
				else ing.Amount = entry.Value;
			}
			foreach (var i in doesntExistIgredients)
			{
				var recIng = new RecipeIngredients
				{
					RecipeId = recipeInDb.RecipeId,
					IngredientId =i.Key.IngredientId,
					Amount = i.Value
				};
				await _context.RecipeIngredients.AddAsync(recIng);
			}
			
			_context.Entry(recipeInDb).State = EntityState.Modified;
			_context.SaveChanges();
			return recipeDTO;
		}
	}
}
