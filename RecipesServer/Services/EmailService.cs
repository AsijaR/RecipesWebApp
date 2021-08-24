using Microsoft.Extensions.Options;
using RecipesServer.DTOs.Order;
using RecipesServer.Helpers;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RecipesServer.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _emailSettings;
		public EmailService(IOptions<EmailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value;
		}
		public bool SendConfirmationEmail(string userEmail, string confirmationLink)
		{
			string fromMail = _emailSettings.Email;
			string fromPassword = _emailSettings.Password;

			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(fromMail);
			mailMessage.To.Add(new MailAddress(userEmail));

			mailMessage.Subject = "Confirm your email";
			mailMessage.IsBodyHtml = true;
			mailMessage.Body = "<html><body> <p>Please click on this link to confirm your email</p>" + confirmationLink + " </body></html>";

			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential(fromMail, fromPassword),
				EnableSsl = true,
			};
			try
			{
				smtpClient.Send(mailMessage);
				return true;
			}
			catch (Exception e)
			{

				return false;
			}

		}
		public bool SendOrderEmail(string userEmail, Recipe recipe, RecipeOrders order,float shippingPrice)
		{
			string fromMail = _emailSettings.Email;
			string fromPassword = _emailSettings.Password;

			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(fromMail);
			mailMessage.To.Add(new MailAddress(userEmail));

			mailMessage.Subject = "Order";
			mailMessage.IsBodyHtml = true;
			mailMessage.Body = 
				"<html><body> " +
					"<h2>You have just made an order!</h2>"+
					"</br>"+
					"<h4>Here is your order information</h4>"+
					"</br>" +
					"<p>Full Name: </p>"+ order.Order.FullName+
					"</br>" +
					"<p>Shipping Address</p>" + order.Order.Address +" "+order.Order.City+" "+order.Order.State+" "+ order.Order.Zip+"</br>"+
					"<p>Order information</p></br>" +
					"<p>Meal:"+recipe.Title + "</p>"+ "</br>" +
					"<p>Note from chef: </p>" + recipe.NoteForShipping + "</br>" +
					"<p>Date meal should be shipped: </p>" + order.Order.DateMealShouldBeShipped.Date + "</br>" +
					"<p>Price: </p>" +"$"+recipe.Price + "</br>" +
					"<p>Serving Number: </p>" + order.Order.ServingNumber + "</br>" +
					"<p>Shipping price: </p>" + "$" + shippingPrice + "</br>" +
					"<p>Total price to be paid: </p>" + "$" + order.Order.Total + "</br>" +
					"<p>Message to chef: </p>" + order.Order.NoteToChef + "</br></br>" +
				"<h5>Currenty your order is on waiting. We'll let you know when chef changes its status.</h5>" 
				+ " </body></html>";

			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential(fromMail, fromPassword),
				EnableSsl = true,
			};
			try
			{
				smtpClient.Send(mailMessage);
				return true;
			}
			catch (Exception e)
			{

				return false;
			}

		}
		public bool OrderStatusEmail(string userEmail,string order)
		{
			string fromMail = _emailSettings.Email;
			string fromPassword = _emailSettings.Password;

			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(fromMail);
			mailMessage.To.Add(new MailAddress(userEmail));

			mailMessage.Subject = "Order Status";
			mailMessage.IsBodyHtml = true;
			mailMessage.Body =
				"<html><body> " +
				"<h2>The chef has changed your order status to " +order+ "!</h2>" + " </body></html>";

			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential(fromMail, fromPassword),
				EnableSsl = true,
			};
			try
			{
				smtpClient.Send(mailMessage);
				return true;
			}
			catch (Exception e)
			{

				return false;
			}

		}
	}
}
