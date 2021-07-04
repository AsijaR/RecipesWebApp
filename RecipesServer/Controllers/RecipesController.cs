using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipesServer.DTOs.Comment;
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
	public class RecipesController : BaseApiController
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;
		private readonly IPhotoService photoService;
		public RecipesController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
			this.photoService = photoService;
		}

		[HttpGet("{id}", Name = "GetRecipe")]
		public async Task<ActionResult<RecipeDTO>> GetRecipe(int id)
		{
			return await unitOfWork.RecipeRepository.GetRecipeByIdAsync(id);
		}
		[HttpGet("search-recipes")]
		public async Task<ActionResult<IEnumerable<RecipeBasicInfoDTO>>> GetSearchedRecipes([FromQuery]RecipeParams recipeParams)
		{
			var recipes = await unitOfWork.RecipeRepository.GetSearchedRecipesAsync(recipeParams);
			Response.AddPaginationHeader(recipes.CurrentPage,recipes.PageSize,recipes.TotalCount,recipes.TotalPages);
			return Ok(recipes);
		}
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<IEnumerable<RecipeBasicInfoDTO>>> GetUserRecipes()
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			var r= await unitOfWork.RecipeRepository.GetUserRecipes(user.Id);
			return r.ToList();
		}
		[Authorize]
		[HttpPut("edit-recipe/{id}")]
		public async Task<ActionResult> EditRecipe(int id,RecipeIngDTO recipeIngDTO)
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			
			var recipe = await unitOfWork.RecipeRepository.FindRecipeByIdAsync(id);
			if (recipe != null)
			{
				if (recipe.UserId != user.Id) return Forbid("You are not authorized to edit this recipe!!");

				//mapper.Map(recipeDTO, recipe);
				await unitOfWork.RecipeRepository.UpdateRecipe(recipe,recipeIngDTO);
				 return Ok("Recipe Is updated.");
			}
			return BadRequest("Failed to update user");
		}
		[Authorize]
		[HttpPost]
		public async Task<ActionResult<Int32>> AddRecipe(RecipeIngDTO recipeDTO)
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			recipeDTO.Recipe.UserId = user.Id;
			var id=await this.unitOfWork.RecipeRepository.AddNewRecipe(recipeDTO);
			return id;
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
		[HttpPost("add-photo/{id}")]
		public async Task<ActionResult<RecipePhotoDTO>> AddPhoto(int id,IFormFile file)
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			var recipe = await unitOfWork.RecipeRepository.FindRecipeByIdAsync(id);
			
			var result = await photoService.AddRecipePhotoAsync(file);

			if (result.Error != null) return BadRequest(result.Error.Message);

			var photo = new RecipePhotos
			{
				Url = result.SecureUrl.AbsoluteUri,
				PublicId = result.PublicId
			};
			if (recipe.UserId == user.Id) 
			{
				unitOfWork.RecipeRepository.deleteRecipeHeaderPreviousPhoto(recipe.RecipeId);
				photo.IsMain = true;
			}
			recipe.RecipePhotos.Add(photo);

			if (await unitOfWork.Complete())
			{
				return Ok(mapper.Map<RecipePhotoDTO>(photo));
			}

			return BadRequest("Problem adding photo");
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
					if (user.Id == comm.AppUserId)
					{
						unitOfWork.CommentRepository.DeleteComment(id, commentId);
						unitOfWork.CommentRepository.DeleteCommentAsync(comm);
						await unitOfWork.Complete();
					}
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
