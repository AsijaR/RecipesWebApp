using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Errors
{
	public class APIException
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public string Details { get; set; }

		public APIException(int statusCode, string meesage, string details = "")
		{
			this.StatusCode = statusCode;
			this.Message = meesage;
			this.Details = details;
		}
	}
}
