﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Helpers
{
	public class RecipeParams : PaginationParams
    {
        public string CurrentId { get; set; }
        //public string Gender { get; set; }
      //  public int MinAge { get; set; } = 18;
      //  public int MaxAge { get; set; } = 150;
        public string OrderBy { get; set; } = "lastAdded";
    }
}
