using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipesServer.DTOs;
using RecipesServer.Extensions;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Controllers
{
	[Authorize]
	public class BookmarkController : BaseApiController
	{
		private readonly IUnitOfWork unitOfWork;

		public BookmarkController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

        [HttpGet]
        public async Task<ActionResult<BookmarkDTO>> GetUserBookmark()
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
            var bk = this.unitOfWork.BookmarkRepository.GetUserBookmarkId(user.Id);
            var result = await unitOfWork.BookmarkRepository.GetUserBookmark(bk);
			if (result == null) return NotFound();
			return result;
		}

        [HttpPut("add-to-bookmark/{id}")]
        public async Task<ActionResult<Recipe>> AddRecipeToBookmark(int id)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
            var bk = this.unitOfWork.BookmarkRepository.GetUserBookmarkId(user.Id);
            var exist = unitOfWork.BookmarkRepository.RecipeExistInBookmark(bk,id);
			if (!exist.Result) { 
                var r= await unitOfWork.BookmarkRepository.AddToBookmark(bk, id);
                if (r == null) return BadRequest("nije kkako treba");
                else
                {
                    await unitOfWork.Complete();
                    return Ok("Recipe is added to bookmark");
                }
            }
            else return Ok("Recipe already exist in bookmark");
        }

        [HttpPut("remove-from-bookmark/{id}")]
        public async Task<ActionResult<Recipe>> RemoveRecipeFromBookmark(int id)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
            var bk = this.unitOfWork.BookmarkRepository.GetUserBookmarkId(user.Id);
            var exist =  unitOfWork.BookmarkRepository.RecipeExistInBookmark(bk, id);
            if (exist.Result)
            {
                unitOfWork.BookmarkRepository.DeleteFromBookmark(bk, id);
               // if (r == null) return BadRequest("nije kkako treba");
                await unitOfWork.Complete();
                return Ok("Recipe is removed from bookmark");
            }
            else return BadRequest("Doesnt Exists");
        }

    }
}
