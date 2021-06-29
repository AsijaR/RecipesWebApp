using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Helpers
{
    public class RecipeParams : PaginationParams
    {
        public string Title { get; set; }
        public bool searchIng { get; set; }
        public string Ingredient1 { get; set; }
        public string Ingredient2 { get; set; }
        public string Ingredient3 { get; set; }
    }
}
