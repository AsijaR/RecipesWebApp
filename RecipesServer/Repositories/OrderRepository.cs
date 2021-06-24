using AutoMapper;
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
		public async Task<IEnumerable<CustomRecipeOrder>> GetChefOrders(int chefId)
		{
			var o = await _context.RecipeOrders.Where(ch => ch.ChefId == chefId)
												.Include(o => o.Order).
												Join(_context.Recipes, ord => ord.Order.RecipeId, rec => rec.RecipeId, (ord, rec) =>
														new CustomRecipeOrder { OrderId = ord.OrderId, Title = rec.Title, Price = rec.Price, Order = ord.Order, ApprovalStatus = ord.ApprovalStatus })
												.ToArrayAsync();
			return o;
		}
		public async Task<RecipeOrders> EditOrder(int chefId, int orderId, string orderStatus)
		{
			var findOrder = await _context.RecipeOrders
								.Where(o => o.ChefId == chefId && o.OrderId == orderId)
								.FirstOrDefaultAsync();
			if (findOrder != null)
			{
				if (orderStatus == "Approved" || orderStatus == "Denied" || orderStatus == "Completed" || orderStatus == "Waiting")
				{
					findOrder.ApprovalStatus = orderStatus;
					await _context.SaveChangesAsync();
				}
			}
			return findOrder;
		}

		public async Task<IEnumerable<CustomRecipeOrder>> SortChefsOrder(int chefId, string status)
		{
			if (status == "all" || status==null)
			{
				return await _context.RecipeOrders.Where(ch => ch.ChefId == chefId)
												.Include(o => o.Order).
												Join(_context.Recipes, ord => ord.Order.RecipeId, rec => rec.RecipeId, (ord, rec) =>
														new CustomRecipeOrder { OrderId = ord.OrderId, Title = rec.Title, Price = rec.Price, Order = ord.Order, ApprovalStatus = ord.ApprovalStatus })
												.ToArrayAsync();
			}
			else
			{
				return await _context.RecipeOrders.Where(ch => ch.ChefId == chefId)
													.Include(o => o.Order).Where(p => p.ApprovalStatus == status).
													Join(_context.Recipes, ord => ord.Order.RecipeId, rec => rec.RecipeId, (ord, rec) =>
															new CustomRecipeOrder { OrderId = ord.OrderId, Title = rec.Title, Price = rec.Price, Order = ord.Order, ApprovalStatus = ord.ApprovalStatus })
													.ToArrayAsync();
			}
		}

		public async Task<OrderDTO> OrderMeal(int userId, OrderDTO order)
		{
			var findRecipe = await _context.Recipes.FindAsync(order.RecipeId);
			var o = _mapper.Map<Order>(order);
			if (findRecipe != null)
			{
				var makeOrder = await _context.Orders.AddAsync(o);
				_context.SaveChanges();
				_context.RecipeOrders.Add(new RecipeOrders { 
					UserId = userId, 
					ChefId = findRecipe.UserId, 
					OrderId = makeOrder.Entity.OrderId
				}
				);
				_context.SaveChanges();
			}
			return null;
		}
	}
}
