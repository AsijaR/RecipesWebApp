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
        void Update(Recipe recipe);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task<RecipeDTO> GetRecipeByIdAsync(int id);
       // Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagedList<RecipeDTO>> GetRecipesAsync(RecipeParams recipeParams);
       // Task<Recipe> GetRecipeAsync(int recipeId);
        Task<Recipe> FindRecipeByIdAsync(int recipeId);
        public int ingredientExists(Ingredient ingredient);
    }
}
