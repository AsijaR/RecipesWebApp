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

		public OrderRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public async Task<PagedList<GetOrdersDTO>> GetChefOrders(int chefId,OrderParams orderParams)
		{
			var orders =  _context.RecipeOrders.Where(ch => ch.ChefId == chefId)
												.Include(o => o.Order).Include(r=>r.Recipe).Include(c=>c.Chef)
												//.Join(_context.Recipes,order=> order.OrderId, recipe=>recipe.RecipeId, (order,recipe)=>new { Recipe=recipe,Order=order})
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

			if (findOrder != null)
			{
				if (orderStatus.Status == "Approved" || orderStatus.Status == "Denied" || orderStatus.Status == "Completed" || orderStatus.Status == "Waiting")
				{
					findOrder.ApprovalStatus = orderStatus.Status;
					await _context.SaveChangesAsync();
				}
			}
			return _mapper.Map<OrderStatusDTO>(findOrder);
		}

		public async Task<MakeOrderDTO> OrderMeal(int userId, MakeOrderDTO order)
		{
			var findRecipe = await _context.Recipes.FirstOrDefaultAsync(r=>r.RecipeId==order.RecipeId);
			var o = _mapper.Map<RecipeOrders>(order);
			if (findRecipe != null)
			{
				var makeOrder = await _context.Orders.AddAsync(o.Order);
				await _context.SaveChangesAsync();
				_context.RecipeOrders.Add(new RecipeOrders { 
					UserId = userId, 
					ChefId = findRecipe.AppUserId, 
					OrderId = makeOrder.Entity.OrderId,
					RecipeId=order.RecipeId
				}
				);
				await _context.SaveChangesAsync();
			}
			return order;
		}
	}
}
