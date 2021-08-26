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
		private readonly IEmailService emailService;
		public OrderController(IUnitOfWork unitOfWork, IEmailService emailService)
		{
			this.unitOfWork = unitOfWork;
			this.emailService = emailService;
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
		[HttpGet("users-orders")]
		public async Task<ActionResult<IEnumerable<GetOrdersDTO>>> GetUsersOrders([FromQuery] OrderParams orderParams)
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
			var emailResponse = await unitOfWork.OrderRepository.OrderMeal(user.Id,user.Email, order);
			if (emailResponse)
				return Ok("Meal is succefuly ordered. Check your email.");
			else
			{
				return BadRequest("Ups something bad happend");
			}
		}

	}
}
