using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs.Member
{
	public class ChangePasswordDTO
	{
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
		public string NewPassword2 { get; set; }
	}
}
