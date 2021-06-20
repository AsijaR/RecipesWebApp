using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipesServer.DTOs;
using RecipesServer.Extensions;
using RecipesServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UserController(IUserRepository userRepository, IMapper mapper)
		{
		    this._userRepository = userRepository;
			this._mapper = mapper;
		}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
           // var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
           

           var users = await _userRepository.GetAllUsersAsync();

           

            return Ok(users);
        }
    }
}
