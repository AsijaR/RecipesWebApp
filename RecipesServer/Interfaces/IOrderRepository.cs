using RecipesServer.DTOs.Order;
using RecipesServer.Helpers;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface IOrderRepository
	{
		Task<bool> OrderMeal(int userId, string email,MakeOrderDTO order);
		Task<OrderStatusDTO> EditOrder(int chefId, int orderId, OrderStatusDTO orderStatus);
		Task<PagedList<GetOrdersDTO>> GetChefOrders(int userId,OrderParams orderParams);
	}
}
