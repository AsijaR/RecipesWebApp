using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipesServer.DTOs;
using RecipesServer.DTOs.Recipe;
using RecipesServer.Extensions;
using RecipesServer.Helpers;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Controllers
{
	public class AdminController : BaseApiController
	{
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        public AdminController(UserManager<AppUser> userManager,IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-all-users")]
        public async Task<ActionResult<IEnumerable<UserInfoDTO>>> GetUsers()
        {
            var users = await unitOfWork.UserRepository.GetAllUsersAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete-user/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = unitOfWork.UserRepository.deleteUser(id);

            if (!user) return NotFound("Could not find user");
            await unitOfWork.Complete();
            return Ok("User is deleted");
 
          
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-all-recipes")]
        public async Task<ActionResult<IEnumerable<RecipeBasicInfoDTO>>> GetRecipes()
        {
            var rParams = new RecipeParams();
            rParams.Title = "";
            var recipes = await unitOfWork.RecipeRepository.GetSearchedRecipesAsync(rParams);
            Response.AddPaginationHeader(recipes.CurrentPage, recipes.PageSize, recipes.TotalCount, recipes.TotalPages);
            return Ok(recipes);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete-recipe/{id}")]
        public async Task<ActionResult> DeleteRecipe(int id)
        {
            var recipe = await unitOfWork.RecipeRepository.FindRecipeByIdAsync(id);
            unitOfWork.RecipeRepository.DeleteRecipe(recipe);
            return Ok("User is deleted");

        }
    }
}
