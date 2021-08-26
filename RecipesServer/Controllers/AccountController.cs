using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecipesServer.DTOs;
using RecipesServer.DTOs.Member;
using RecipesServer.Extensions;
using RecipesServer.Helpers;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using RecipesServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RecipesServer.Controllers
{
	public class AccountController : BaseApiController
	{
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager; 
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper,IUnitOfWork unitOfWork, IEmailService emailService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.tokenService = tokenService;
            this.emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username)) return BadRequest("Username is taken");

            var user = mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.Username.ToLower();

            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded) 
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                string confirmationLink = Url.Action("ConfirmEmail","Account", new { userid = user.Id,  token = token }, protocol: HttpContext.Request.Scheme);
                //EmailHelper emailHelper = new EmailHelper();
                bool emailResponse = emailService.SendConfirmationEmail(user.Email, confirmationLink);
                unitOfWork.UserRepository.CreateUserBookmark(user.Id);
                var roleResult = await userManager.AddToRoleAsync(user, "Member"); 
                if (!roleResult.Succeeded) return BadRequest(result.Errors);
                if (emailResponse)
                    return Ok("Confirmation link has been sended to your email.");
                else
                {
                    return BadRequest("Ups something bad happend");
                }
            }
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok("dobro je");

        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string userid)
        {
            var user = await userManager.FindByIdAsync(userid);
            if (user == null)
                return BadRequest("Something went wrong. Please try later");

            var result = await userManager.ConfirmEmailAsync(user, token);
            return Ok(result.Succeeded ? "Your email has been confirmed" : "Error");
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            var result = await signInManager
                .CheckPasswordSignInAsync(user, loginDTO.Password, false);
            //if (loginDTO.Username.ToLower() != "admin") 
            //{ 
                if (result.IsNotAllowed) 
                    return BadRequest("Please confirm your email to access your account");
            //}
            if (!result.Succeeded) return Unauthorized();
            return new UserDTO
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user),
                UserId = user.Id
                };
        }

        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        [HttpPut("change-password")]
        public async Task<ActionResult<ChangePasswordDTO>> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());

            var check = await userManager.CheckPasswordAsync(user, changePasswordDTO.CurrentPassword);
            if (check) 
            {
                if(changePasswordDTO.CurrentPassword== changePasswordDTO.NewPassword) return BadRequest("Old password cannot be the same as the new one.");
                await userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword); 
            }
                
            else
                return BadRequest("Current password is not correct.");
            return NoContent();
            
        }

    }
}
