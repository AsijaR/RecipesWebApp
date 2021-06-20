﻿using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface ITokenService
	{
		Task<string> CreateToken(AppUser user);
	}
}
