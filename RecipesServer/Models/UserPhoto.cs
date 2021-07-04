using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Models
{
	public class UserPhoto
	{
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public int AppUserId { get; set; }
        public AppUser User { get; set; }
    }
}
