using AutoMapper;
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

		[HttpGet("{id}", Name = "GetRecipeDTO")]
		public async Task<ActionResult<RecipeDTO>> GetRecipe(int id=1)
		{
			return await unitOfWork.RecipeRepository.GetRecipeByIdAsync(id);
		}
		[HttpGet]
		public async Task<ActionResult<Recipe>> GetUser(int id=1)
		{
			//var r= await unitOfWork.RecipeRepository.GetRecipeAsync(id);
			return Ok();
		}
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

	}
}
