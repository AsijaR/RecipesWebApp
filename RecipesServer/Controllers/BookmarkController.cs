using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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


        [HttpPut("add-to-bookmark/{id}")]
        public async Task<ActionResult<Recipe>> AddRecipeToBookmark(int id)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
            var bk = this.unitOfWork.BookmarkRepository.GetUserBookmarkId(user.Id);
            unitOfWork.BookmarkRepository.AddToBookmark(bk, id);
            //var found = this.unitOfWork.BookmarkRepository.RecipeExistInBookmark(bk, recipeId);
            //    if (found)
            //    {
            //         unitOfWork.BookmarkRepository.AddToBookmark(id, recipeId);
            //        return addRecipe;
            //    }
            //    else 
                return Conflict();
            
        }

    }
}
