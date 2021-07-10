using Microsoft.Extensions.Options;
using RecipesServer.Helpers;
using RecipesServer.Interfaces;
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
		public bool SendEmail(string userEmail, string confirmationLink)
		{
			string fromMail = _emailSettings.Email;
			string fromPassword = _emailSettings.Password;

			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(fromMail);
			mailMessage.To.Add(new MailAddress(userEmail));

			mailMessage.Subject = "Confirm your email";
			mailMessage.IsBodyHtml = true;
			mailMessage.Body = "<html><body> <p>Please click on this link to confirm your email</p>" + confirmationLink + " </body></html>"; ;

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
