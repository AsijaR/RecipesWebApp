using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs
{
	public class MemberDTO
	{
		public int Id { get; set; }
		public string Username { get; set; }
		//public string PhotoUrl { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string ShippingPrice { get; set; }
	}
}
