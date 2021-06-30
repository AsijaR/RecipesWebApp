using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RecipesServer.Helpers
{
	public class EmailHelper
	{
		public bool SendEmail(string userEmail, string confirmationLink)
		{
			string fromMail = "brvimmm@gmail.com";
			string fromPassword = "xjimjjstopoxbhfe";

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

