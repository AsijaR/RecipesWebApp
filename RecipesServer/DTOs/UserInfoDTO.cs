using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs
{
	public class UserInfoDTO
	{
		public string Username { get; set; }
		public string FullName { get; set; }
		public int AppUserId { get; set; }
	}
}
