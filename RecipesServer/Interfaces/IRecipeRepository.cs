using RecipesServer.DTOs.Recipe;
using RecipesServer.Helpers;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface IRecipeRepository
	{
        Task<IEnumerable<RecipeBasicInfoDTO>> GetUserRecipes(int userId);
        void DeleteRecipe(Recipe recipe);
        Task<RecipeIngDTO> UpdateRecipe(Recipe recipe, RecipeIngDTO recipeIngDTO); 
        Task<bool> SaveAllAsync();
        Task<IEnumerable<RecipeBasicInfoDTO>> GetAllRecipesAsync();
        Task<Int32> AddNewRecipe(RecipeIngDTO recipeDTO);
        // Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task<RecipeDTO> GetRecipeByIdAsync(int id);
        void deleteRecipeHeaderPreviousPhoto(int recipeId);
        Task<PagedList<RecipeBasicInfoDTO>> GetSearchedRecipesAsync(RecipeParams recipeParams);
        // Task<Recipe> GetRecipeAsync(int recipeId);

        Task<Recipe> FindRecipeByIdAsync(int recipeId);
       // public int ingredientExists(Ingredient ingredient);
    }
}
