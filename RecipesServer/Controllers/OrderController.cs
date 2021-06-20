﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipesServer.DTOs.Order;
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
	public class OrderController : BaseApiController
	{
		private readonly IUnitOfWork unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomRecipeOrder>>> GetChefsOrders(string status)
        {
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			var result = await unitOfWork.OrderRepository.SortChefsOrder(user.Id,status);
			
			if (result == null) return NotFound();

			return Ok(result);

		}
        [HttpPut("change-status/{orderId}")]
        public async Task<ActionResult<OrderStatusDTO>> ChangeOrder(int orderId, string orderStatus)
        {
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());

			var changeOrder = await unitOfWork.OrderRepository.EditOrder(user.Id, orderId, orderStatus);
                if (changeOrder == null) return NotFound();

                return Ok(changeOrder);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> AddOrder(OrderDTO order)
		{
			var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
			if (order == null)
				return BadRequest();
			await unitOfWork.OrderRepository.OrderMeal(user.Id, order);

			return Ok("Order has been made");

		}

	}
}