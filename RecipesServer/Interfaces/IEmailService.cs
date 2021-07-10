using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface IEmailService
	{
		bool SendEmail(string userEmail, string confirmationLink);
	}
}
