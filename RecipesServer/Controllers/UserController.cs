using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipesServer.DTOs;
using RecipesServer.DTOs.Member;
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
    public class UserController : BaseApiController
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;
		private readonly IPhotoService photoService;

		public UserController(IUnitOfWork unitOfWork, IMapper mapper,IPhotoService photoService)
		{
		    this.unitOfWork = unitOfWork;
			this.mapper = mapper;
            this.photoService = photoService;
		}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
           // var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
           

          // var users = await _userRepository.GetAllUsersAsync();

           

            return Ok();
        }
        [HttpGet("userInfo")]
        public async Task<ActionResult<MemberDTO>> GetUserInfo()
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
            //var recipe = unitOfWork.RecipeRepository.FindRecipeByIdAsync(id);
            //if (recipe != null)
            //{
            //    unitOfWork.CommentRepository.AddComment(id, user.Id, message);
            //    await unitOfWork.Complete();
            //    return message;
            //}
            //return BadRequest();

            return Ok(mapper.Map<MemberDTO>(user));
        }
        [HttpPut("updateProfile")]
        public async Task<ActionResult<MemberUpdateProfileDTO>> UpdateProfile(MemberUpdateProfileDTO memberUpdateProfileDTO)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
            mapper.Map(memberUpdateProfileDTO,user);
            unitOfWork.UserRepository.Update(user);

            if (await unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPut("change-order")]
        public async Task<ActionResult<MemberUpdateShippingPriceDTO>> ChangeOrder(MemberUpdateShippingPriceDTO shippingPriceDTO)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
            mapper.Map(shippingPriceDTO, user);
            unitOfWork.UserRepository.Update(user);

            if (await unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<UserPhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
           
            var result = await photoService.AddUserPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new UserPhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            unitOfWork.UserRepository.deleteUserPreviousPhoto(user.Id);

            user.UserPhoto = photo;

            if (await unitOfWork.Complete())
            {
                return Ok(mapper.Map<UserPhoto>(photo));
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("change-password")]
        public async Task<ActionResult<MemberDTO>> ChangePassword()
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
            //var recipe = unitOfWork.RecipeRepository.FindRecipeByIdAsync(id);
            //if (recipe != null)
            //{
            //    unitOfWork.CommentRepository.AddComment(id, user.Id, message);
            //    await unitOfWork.Complete();
            //    return message;
            //}
            //return BadRequest();

            return Ok(mapper.Map<MemberDTO>(user));
        }
    }
}
