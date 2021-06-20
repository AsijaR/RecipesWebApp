using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipesServer.DTOs.Comment;
using RecipesServer.DTOs.Recipe;
using RecipesServer.Extensions;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Controllers
{
	public class RecipesController : BaseApiController
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;

		public RecipesController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
		}

		[HttpGet("{id}", Name = "GetRecipe")]
		public async Task<ActionResult<RecipeDTO>> GetRecipe(int id)
		{
			return await unitOfWork.RecipeRepository.GetRecipeByIdAsync(id);
		}
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<IEnumerable<RecipeBasicInfoDTO>>> GetUserRecipes()
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			var r= await unitOfWork.RecipeRepository.GetUserRecipes(user.Id);
			return r.ToList();
		}

		[HttpPut("edit-recipe/{id}")]
		public async Task<ActionResult> PostRecipe(int id,RecipeUpdateDTO recipeDTO)
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			
			var recipe = await unitOfWork.RecipeRepository.FindRecipeByIdAsync(id);
			if (recipe != null)
			{
				if (recipe.UserId != user.Id) return Forbid("You are not authorized to edit this recipe!!");
				mapper.Map(recipeDTO, recipe);
				unitOfWork.RecipeRepository.Update(recipe);
				if (await unitOfWork.RecipeRepository.SaveAllAsync()) return Ok("okkk");
			}
			return BadRequest("Failed to update user");
		}
		[Authorize]
		[HttpPost]
		public async Task<ActionResult<RecipeIngDTO>> AddRecipe(RecipeIngDTO recipe)
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			recipe.Recipe.UserId = user.Id;
			await this.unitOfWork.RecipeRepository.AddNewRecipe(recipe);
			return Ok();///CreatedAtRoute("GetRecipe", new { recipe = recipe.RecipeId }, mapper.Map<NewRecipeDTO>(recipe));
		}

		[Authorize]
		[HttpPost("{id}/add-comment")]
		public async Task<ActionResult<AddCommentDTO>> AddComment(int id, AddCommentDTO message)
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			var recipe = unitOfWork.RecipeRepository.FindRecipeByIdAsync(id);
			if (recipe != null) { 
				unitOfWork.CommentRepository.AddComment(id,user.Id, message);
				await unitOfWork.Complete();
				return message;
			}
			return BadRequest();
		}
		[Authorize]
		[HttpDelete("{id}/delete-comment")]
		public async Task<ActionResult<AddCommentDTO>> DeleteComment(int id, int commentId)
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			var recipe = unitOfWork.RecipeRepository.FindRecipeByIdAsync(id);
			if (recipe != null)
			{
				var comm = await unitOfWork.CommentRepository.GetCommentAsync(commentId);
				if (comm != null) 
				{
					unitOfWork.CommentRepository.DeleteComment(id, commentId);
					unitOfWork.CommentRepository.DeleteCommentAsync(comm);
					await unitOfWork.Complete(); 
				}
				
				return NoContent();
			}
			return BadRequest();
		}
		[Authorize]
		[HttpDelete]
		public async Task<IActionResult> DeleteRecipe(int id)
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			var recipe = await unitOfWork.RecipeRepository.FindRecipeByIdAsync(id);
			if (recipe != null) 
			{
				if (recipe.UserId != user.Id) return Forbid("You are not authorized to delete this recipe!!");
				unitOfWork.RecipeRepository.DeleteRecipe(recipe);
				return NoContent();
			}
			return BadRequest();
		}
	}
}
