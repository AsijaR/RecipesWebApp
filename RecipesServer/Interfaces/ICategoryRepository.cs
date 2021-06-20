using RecipesServer.DTOs.Category;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface ICategoryRepository
	{
		Task<IEnumerable<AllCategoriesDTO>> GetAllCategories();
		Task<CategoryDTO> GetCategory(int categoryId);
		void UpdateCategory(AllCategoriesDTO category);
		void AddCategory(CategoryUpdateDTO category);
		Task<AllCategoriesDTO> DeleteCategory(int categoryId);

		//Task<Category> AddRecipeToCategory(int categoryId, int recipeId);
		//Task<Category> RemoveRecipeFromCategory(int categoryId, int recipeId);
	}
}
