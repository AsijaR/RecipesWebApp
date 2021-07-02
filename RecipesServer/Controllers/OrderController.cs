using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipesServer.DTOs.Order;
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
	
	public class OrderController : BaseApiController
	{
		private readonly IUnitOfWork unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrdersDTO>>> GetChefsOrders([FromQuery]OrderParams orderParams)
        {
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			var result = await unitOfWork.OrderRepository.GetChefOrders(user.Id, orderParams);
			
			if (result == null) return NotFound();
			Response.AddPaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
			return Ok(result);

		}
		[Authorize]
        [HttpPut("change-status/{orderId}")]
        public async Task<ActionResult<OrderStatusDTO>> ChangeOrder(int orderId, OrderStatusDTO orderStatus)
        {
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());

			var changeOrder = await unitOfWork.OrderRepository.EditOrder(user.Id, orderId, orderStatus);
                if (changeOrder == null) return NotFound();

                return Ok(changeOrder);
        }

        [HttpPost]
		[Authorize]
		public async Task<ActionResult<MakeOrderDTO>> MakeOrder(MakeOrderDTO order)
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			if (order == null)
				return BadRequest("Bad request. Try again later.");
			await unitOfWork.OrderRepository.OrderMeal(user.Id, order);

			return Ok("Meal is succefuly ordered");

		}

	}
}
