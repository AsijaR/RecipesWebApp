using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Helpers
{
	public class OrderParams : PaginationParams
    {
        public string OrderByStatus { get; set; } = "All";
    }
}
