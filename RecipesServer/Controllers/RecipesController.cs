using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipesServer.DTOs.Recipe;
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
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RecipesController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			this._unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet("{id}", Name = "GetRecipeDTO")]
		public async Task<ActionResult<RecipeDTO>> GetRecipe(int id=1)
		{
			return await _unitOfWork.RecipeRepository.GetRecipeByIdAsync(id);
		}
		[HttpGet]
		public async Task<ActionResult<Recipe>> GetUser(int id=1)
		{
			var r= await _unitOfWork.RecipeRepository.GetRecipeAsync(id);
			return Ok(r);
		}

	}
}
