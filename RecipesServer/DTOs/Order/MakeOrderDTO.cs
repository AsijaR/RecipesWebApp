using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.DTOs.Order
{
	public class MakeOrderDTO
	{
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public DateTime DateMealShouldBeShipped { get; set; }
        public int ServingNumber { get; set; }
        public string NoteToChef { get; set; }
        public int RecipeId { get; set; }
        public float Total { get; set; }
    }
}
