using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RecipesServer.Data;
using RecipesServer.DTOs.Order;
using RecipesServer.Helpers;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
		public OrderRepository(DataContext context, IMapper mapper, IEmailService emailService)
		{
			_context = context;
			_mapper = mapper;
			_emailService = emailService;
		}
		public async Task<PagedList<GetOrdersDTO>> GetChefOrders(int chefId,OrderParams orderParams)
		{
			var orders =  _context.RecipeOrders.Where(ch => ch.ChefId == chefId)
												.Include(o => o.Order).Include(r=>r.Recipe).Include(c=>c.Chef)
												.AsQueryable();
			if (orderParams.OrderByStatus != "All")
				orders = orders.Where(o=>o.ApprovalStatus==orderParams.OrderByStatus);
			return await PagedList<GetOrdersDTO>.CreateAsync(orders.ProjectTo<GetOrdersDTO>(_mapper.ConfigurationProvider).AsNoTracking(),
					orderParams.PageNumber, orderParams.PageSize);
		}

		public async Task<OrderStatusDTO> EditOrder(int chefId, int orderId, OrderStatusDTO orderStatus)
		{
			var findOrder = await _context.RecipeOrders
								.Where(o => o.ChefId == chefId && o.OrderId == orderId)
								.FirstOrDefaultAsync();
			var findUser = await _context.RecipeOrders
								.Where(o =>o.OrderId == orderId).Select(u=>u.UserId)
								.FirstOrDefaultAsync();
			var user= await _context.Users.FirstOrDefaultAsync(x => x.Id == findUser);
			if (findOrder != null)
			{
				if (orderStatus.Status == "Approved" || orderStatus.Status == "Denied" || orderStatus.Status == "Completed" || orderStatus.Status == "Waiting")
				{
					findOrder.ApprovalStatus = orderStatus.Status;
					await _context.SaveChangesAsync();
					bool emailResponse = _emailService.OrderStatusEmail(user.Email, orderStatus.Status);
				}
			}
			return _mapper.Map<OrderStatusDTO>(findOrder);
		}

		public async Task<bool> OrderMeal(int userId,string email, MakeOrderDTO order)
		{
			var findRecipe = await _context.Recipes.FirstOrDefaultAsync(r=>r.RecipeId==order.RecipeId);
			var o = _mapper.Map<RecipeOrders>(order);
			if (findRecipe != null)
			{
				var makeOrder = await _context.Orders.AddAsync(o.Order);
				await _context.SaveChangesAsync();
				_context.RecipeOrders.Add(new RecipeOrders { 
					UserId = userId, 
					ChefId = findRecipe.UserId, 
					OrderId = makeOrder.Entity.OrderId,
					RecipeId=order.RecipeId
				}
				);
				await _context.SaveChangesAsync();
			}
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == findRecipe.UserId);
			//bool emailResponse = _emailService.SendOrderEmail(email, findRecipe, o, user.ShippingPrice);
			bool emailResponse = _emailService.SendOrderEmail(email, findRecipe, o, user.ShippingPrice);
			return emailResponse;
		}
	}
}
