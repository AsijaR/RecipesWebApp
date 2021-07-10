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
		public void deleteRecipeHeaderPreviousPhoto(int recipeId)
		{
			var recipeHasHeaderPhoto = _context.RecipePhotos.Where(x => x.RecipeId == recipeId).Any();
			if (recipeHasHeaderPhoto) 
			{
				_context.RecipePhotos.Remove(_context.RecipePhotos.Where(x => x.RecipeId == recipeId).FirstOrDefault(m => m.IsMain == true));
			}
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
			var recipe = await _context.Recipes.Include(c => c.Ingredients)
												.Include(c => c.Comments)
												.ProjectTo<RecipeDTO>(_mapper.ConfigurationProvider)
												.SingleOrDefaultAsync(r => r.RecipeId == id);

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

		public async Task<PagedList<RecipeBasicInfoDTO>> GetSearchedRecipesAsync(RecipeParams recipeParams)
		{
			var query = _context.Recipes.AsQueryable();
			if (recipeParams.Title != null)
			{
				query = query.Where(r => r.Title.Contains(recipeParams.Title));
			}
			if (recipeParams.Ingredient1 != null || recipeParams.Ingredient2 != null || recipeParams.Ingredient3 != null)
			{
				var i1 = _context.RecipeIngredients.Where(i => i.Ingredient.Name.Contains(recipeParams.Ingredient1)).AsNoTracking().Select(r => r.Recipe);
				var i2 = _context.RecipeIngredients.Where(i => i.Ingredient.Name.Contains(recipeParams.Ingredient2)).AsNoTracking().Select(r => r.Recipe);
				var i3 = _context.RecipeIngredients.Where(i => i.Ingredient.Name.Contains(recipeParams.Ingredient3)).AsNoTracking().Select(r => r.Recipe);
				var recipesFoundIn1And2 = i1.Union(i2);
				query = recipesFoundIn1And2.Union(i3);
			}
			return await PagedList<RecipeBasicInfoDTO>.CreateAsync(query.ProjectTo<RecipeBasicInfoDTO>(_mapper.ConfigurationProvider).AsNoTracking(),
					recipeParams.PageNumber, recipeParams.PageSize);
		}

		public async Task<Recipe> FindRecipeByIdAsync(int recipeId)
		{
			return _context.Recipes.Include(i => i.Ingredients)
									.ThenInclude(i => i.Ingredient)
									.Include(p => p.RecipePhotos)
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
					await _context.Ingredients.AddRangeAsync(doesntExistIgredients.Select(p => p.Ingredient));

				var c = _context.Recipes.AddAsync(r);
				try
				{
					_context.SaveChanges();
				}
				catch (DbUpdateException ex)
				{
					var e = ex;
				}

				foreach (KeyValuePair<int, string> entry in alreadyExistIgredients)
				{
					var recIng = new RecipeIngredients
					{
						RecipeId = r.RecipeId,
						IngredientId = entry.Key,
						Amount = entry.Value
					};
					_context.Add(recIng);
				}

				foreach (var i in doesntExistIgredients)
				{
					var recIng = new RecipeIngredients
					{
						RecipeId = r.RecipeId,
						IngredientId = i.Ingredient.IngredientId,
						Amount = i.Amount
					};
					await _context.AddAsync(recIng);
					await _context.SaveChangesAsync();
				}
				_context.Entry(r).GetDatabaseValues();

			}
			return r.RecipeId;
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<RecipeIngDTO> UpdateRecipe(Recipe recipeInDb, RecipeIngDTO recipeIngDTO)
		{
			var mappedRecipe = _mapper.Map<Recipe>(recipeIngDTO.Recipe);
			//var ingredients = _mapper.Map<RecipeIngredients>(recipeIngDTO.Ingredients);
			//recipeInDb.RecipePhotos.Where(pic=>pic.IsMain==true).Select(pic=>pic.Url=recipeDTO.u)
			recipeInDb.Title = mappedRecipe.Title;
			recipeInDb.CategoryId = mappedRecipe.CategoryId;
			recipeInDb.Complexity = mappedRecipe.Complexity;
			recipeInDb.Description = mappedRecipe.Description;
			recipeInDb.MealCanBeOrdered = mappedRecipe.MealCanBeOrdered;
			recipeInDb.Note = mappedRecipe.Note;
			recipeInDb.NoteForShipping = mappedRecipe.NoteForShipping;
			recipeInDb.Price = mappedRecipe.Price;
			recipeInDb.ServingNumber = mappedRecipe.ServingNumber;
			recipeInDb.TimeNeededToPrepare = mappedRecipe.TimeNeededToPrepare;
			var alreadyExistIgredients = new Dictionary<int, string>();
			var doesntExistIgredients = new List<RecipeIngredients>();

			var ingredientsToRemove = new List<RecipeIngredients>();
			var ingredientsWhoStays = new List<RecipeIngredients>();
			//ovo su mi original ingredients od recepta
			//var p=newRecipe.Ingredients.Except((IEnumerable<RecipeIngredients>)ingredients);
			foreach (var ing in recipeInDb.Ingredients)
			{
				var c = recipeIngDTO.Ingredients.Any(i => i.Name == ing.Ingredient.Name);
				if (!c)
					ingredientsToRemove.Add(ing);
				else
					ingredientsWhoStays.Add(ing);
			}
			//da li su mi svi ingredienti isti 
			if (!recipeInDb.Ingredients.Equals(recipeIngDTO.Ingredients))
			{
				//var o = recipeInDb.Ingredients.Select(i=>i.Ingredient).Except((IEnumerable<Ingredient>)_mapper.Map<Ingredient>(recipeIngDTO.Ingredients));
				foreach (var i in recipeIngDTO.Ingredients)
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

				if (ingredientsToRemove.Count() > 0)
					ingredientsToRemove.ForEach(x => recipeInDb.Ingredients.Remove(x));

				if (doesntExistIgredients.Count > 0)
					await _context.Ingredients.AddRangeAsync(doesntExistIgredients.Select(p => p.Ingredient));

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
					else if (ing.Amount==entry.Value) { continue; }
					else ing.Amount = entry.Value;
				}
				foreach (var i in doesntExistIgredients)
				{
					var recIng = new RecipeIngredients
					{
						RecipeId = recipeInDb.RecipeId,
						IngredientId = i.IngredientId,
						Amount = i.Amount
					};
					await _context.RecipeIngredients.AddAsync(recIng);
				}
			}
			_context.Entry(recipeInDb).State = EntityState.Modified;
			_context.SaveChanges();
			return recipeIngDTO;
		}
	}
}
