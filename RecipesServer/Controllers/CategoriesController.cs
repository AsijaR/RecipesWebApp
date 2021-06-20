using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipesServer.DTOs;
using RecipesServer.DTOs.Category;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryUpdateDTO>>> GetCategories()
        {

            var result = await this.unitOfWork.CategoryRepository.GetAllCategories();
            return Ok(result);

        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {

            var result = await this.unitOfWork.CategoryRepository.GetCategory(id);

            if (result == null) return NotFound();
            return Ok(result);

        }
		[HttpPut]
		public async Task<ActionResult<AllCategoriesDTO>> UpdateCategory(AllCategoriesDTO category)
		{

			this.unitOfWork.CategoryRepository.UpdateCategory(category);
			if (await unitOfWork.Complete()) return NoContent();

			return BadRequest("Failed to update category");

		}
		[HttpPost("add-category")]
        public async Task<ActionResult<CategoryUpdateDTO>> CreateCategory(CategoryUpdateDTO category)
        {
                if (category == null)
                    return BadRequest();
                unitOfWork.CategoryRepository.AddCategory(category);
                await unitOfWork.Complete();
                return Ok(category);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var categoryToDelete = await unitOfWork.CategoryRepository.DeleteCategory(id);
            if (categoryToDelete == null) return NotFound($"Category with Id = {id} not found");

                return Ok("Category successfully deleted");
        }

    }
}
