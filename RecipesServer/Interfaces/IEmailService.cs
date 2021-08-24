using RecipesServer.DTOs.Order;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface IEmailService
	{
		bool SendConfirmationEmail(string userEmail, string confirmationLink);
		bool SendOrderEmail(string userEmail,Recipe recipe, RecipeOrders order, float shippingPrice);
		bool OrderStatusEmail(string userEmail, string order);
	}
}
