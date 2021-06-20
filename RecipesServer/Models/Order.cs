using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class Order
	{
        [JsonIgnore]
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateMealShouldBeShipped { get; set; }
        public int ServingNumber { get; set; }
        public string NoteToChef { get; set; }
        public float ShippingPrice { get; set; }
        public float Total { get; set; }
        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
       // [JsonIgnore]
        public ICollection<RecipeOrders>? Recipes { get; set; }
    }
}
