using RecipesServer.DTOs.Order;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface IOrderRepository
	{
		Task<OrderDTO> OrderMeal(int userId, OrderDTO order);
		Task<RecipeOrders> EditOrder(int chefId, int orderId, string orderStatus);
		Task<IEnumerable<CustomRecipeOrder>> GetChefOrders(int userId);
		Task<IEnumerable<CustomRecipeOrder>> SortChefsOrder(int chefId, string status);
	}
}
