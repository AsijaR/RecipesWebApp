using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Data
{
	public class DataContext : IdentityDbContext<AppUser, AppRole, int,
	 IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
	 IdentityRoleClaim<int>, IdentityUserToken<int>>
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Bookmark> Bookmarks { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<RecipeIngredients> RecipeIngredients { get; set; }
		public DbSet<RecipeBookmarks> RecipeBookmarks { get; set; }
		public DbSet<RecipeComments> RecipeCommments { get; set; }
		public DbSet<RecipeOrders> RecipeOrders { get; set; }
		public DbSet<UserPhoto> UserPhotos { get; set; }
		public DbSet<RecipePhotos> RecipePhotos { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<AppUser>()
				.HasMany(ur => ur.UserRoles)
				.WithOne(u => u.User)
				.HasForeignKey(ur => ur.UserId)
				.IsRequired();

			builder.Entity<AppRole>()
				.HasMany(ur => ur.UserRoles)
				.WithOne(u => u.Role)
				.HasForeignKey(ur => ur.RoleId)
				.IsRequired();

			builder.Entity<RecipeIngredients>().HasKey(x => new { x.RecipeId, x.IngredientId });

			builder.Entity<RecipeIngredients>()
				.HasOne(r => r.Recipe)
				.WithMany(ri => ri.Ingredients)
				.HasForeignKey(r => r.RecipeId);

			builder.Entity<RecipeIngredients>()
				.HasOne(i => i.Ingredient)
				.WithMany(ri => ri.Recipes)
				.HasForeignKey(i => i.IngredientId);

			builder.Entity<RecipeBookmarks>().HasKey(x => new { x.RecipeId, x.BookmarkId });

			builder.Entity<RecipeBookmarks>()
				.HasOne(r => r.Recipe)
				.WithMany(rb => rb.Bookmarks)
				.HasForeignKey(r => r.RecipeId);

			builder.Entity<RecipeBookmarks>()
				.HasOne(b => b.Bookmark)
				.WithMany(rb => rb.Recipes)
				.HasForeignKey(b => b.BookmarkId);

			builder.Entity<RecipeComments>().HasKey(x => new { x.RecipeId, x.CommentId });

			builder.Entity<RecipeComments>()
				.HasOne(r => r.Recipe)
				.WithMany(rb => rb.Comments)
				.HasForeignKey(r => r.RecipeId);
			

			builder.Entity<RecipeComments>()
				.HasOne(b => b.Comment)
				.WithMany(rb => rb.Recipes)
				.HasForeignKey(b => b.CommentId);
			//builder.Entity<AppUser>().HasMany(c=>c.Comments).WithOne(u=>u.User).HasForeignKey(s=>s.UserId).OnDelete(DeleteBehavior.Cascade);

				builder.Entity<RecipeComments>().HasKey(x => new { x.RecipeId, x.CommentId });

				builder.Entity<RecipeComments>()
					.HasOne(r => r.Recipe)
					.WithMany(rc => rc.Comments)
					.HasForeignKey(r => r.RecipeId);

				builder.Entity<RecipeComments>()
					.HasOne(c => c.Comment)
					.WithMany(rc => rc.Recipes)
					.HasForeignKey(c => c.CommentId)
					.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Comment>().HasOne(d => d.User)
				   .WithMany(p => p.Comments)
				   .HasForeignKey(d => d.AppUserId)
				   .OnDelete(DeleteBehavior.Restrict);

			//builder.Entity<RecipeOrders>().HasOne(d => d.Order)
			//		   .WithMany(p => p.)
			//		   .HasForeignKey(d => d.AppUserId)
			//		   .OnDelete(DeleteBehavior.Restrict);
			//builder.Entity<Comment>().HasOne(r=>)

			builder.Entity<RecipeOrders>().HasKey(x => new { x.OrderId, x.ChefId });

			builder.Entity<Category>().HasData(
				new Category { CategoryId = 1, Name = "Lunch" },
					new Category { CategoryId = 2, Name = "Salads" },
					new Category { CategoryId = 3, Name = "Main Dishes" },
					new Category { CategoryId = 4, Name = "Desserts" },
					new Category { CategoryId = 5, Name = "Smoothies" },
					new Category { CategoryId = 6, Name = "Other" }
				);
			//builder.Entity<Ingredient>().HasData(
			//	new Ingredient { IngredientId = 1, Name = "butter" },
			//	new Ingredient { IngredientId = 2, Name = "vegetable oil" },
			//	new Ingredient { IngredientId = 3, Name = "red onion" },
			//	new Ingredient { IngredientId = 4, Name = "shallots" },
			//	new Ingredient { IngredientId = 5, Name = "curry powder" },
			//	new Ingredient { IngredientId = 6, Name = "tomato paste" },
			//	new Ingredient { IngredientId = 7, Name = "coconut milk" },
			//	new Ingredient { IngredientId = 8, Name = "water" },
			//	new Ingredient { IngredientId = 9, Name = "chicken" },
			//	new Ingredient { IngredientId = 10, Name = "lime juice" }
			//	);
			//builder.Entity<Recipe>().HasData(
			//	new Recipe
			//	{
			//		RecipeId = 1,
			//		Title = "Coconut Curry Chicken",
			//		ServingNumber = 5,
			//		TimeNeededToPrepare = "35m",
			//		Description = "In a large pot or high-sided skillet over medium heat, heat oil and butter. When butter is" +
			//					" melted, add onion and shallots and cook until tender and translucent, 6 to 8 minutes.;Add garlic," +
			//					" ginger, and curry powder and cook until fragrant, 1 minute more. Add tomato paste and cook until" +
			//					" darkened slightly, 1 to 2 minutes more.;Add coconut milk and water and bring to a simmer. Add chicken " +
			//					"and cook, stirring occasionally, until chicken is cooked through, 6 to 8 minutes.;Stir in lime juice " +
			//					"and garnish with mint and cilantro. Serve hot with rice.",
			//		Note = "And don't forget to whip up some rice to soak up all that saucy goodness! ",
			//		CategoryId = 1,
			//		MealCanBeOrdered = true,
			//		Price = 10,
			//		NoteForShipping = "needs to be in freezer after deliver",
			//		UserId = 1,
			//		Complexity = "Simple"
			//	});
			//builder.Entity<RecipeIngredients>().HasData(
			//	new RecipeIngredients { RecipeId = 1, Amount = "1 tbsp", IngredientId = 1 },
			//	new RecipeIngredients { RecipeId = 1, Amount = "1 tbsp", IngredientId = 2 },
			//	new RecipeIngredients { RecipeId = 1, Amount = "2 large", IngredientId = 3 },
			//	new RecipeIngredients { RecipeId = 1, Amount = "1 tbsp", IngredientId = 4 },
			//	new RecipeIngredients { RecipeId = 1, Amount = "1.5 tbsp", IngredientId = 5 },
			//	new RecipeIngredients { RecipeId = 1, Amount = "2 tbsp", IngredientId = 6 },
			//	new RecipeIngredients { RecipeId = 1, Amount = "1 can", IngredientId = 7 },
			//	new RecipeIngredients { RecipeId = 1, Amount = "500 ml", IngredientId = 8 },
			//	new RecipeIngredients { RecipeId = 1, Amount = "1.5 lb boneless", IngredientId = 9 },
			//	new RecipeIngredients { RecipeId = 1, Amount = "0.5ml", IngredientId = 10 }
			//);
			/*	builder.Entity<Bookmark>().HasData(
					new Bookmark { BookmarkId = 1, UserId = 1 });
				builder.Entity<Category>().HasData(
					new Category { CategoryId = 1, Name = "Lunch" }
				//	new Category { CategoryId = 2, Name = "Salads" },
				//	new Category { CategoryId = 3, Name = "Main Dishes" },
				//	new Category { CategoryId = 4, Name = "Desserts" },
				//	new Category { CategoryId = 5, Name = "Smoothies" },
				//	new Category { CategoryId = 6, Name = "Other" }
					);
				builder.Entity<Ingredient>().HasData(
					new Ingredient { IngredientId = 1, Name = "butter" },
					new Ingredient { IngredientId = 2, Name = "vegetable oil" },
					new Ingredient { IngredientId = 3, Name = "red onion" },
					new Ingredient { IngredientId = 4, Name = "shallots" },
					new Ingredient { IngredientId = 5, Name = "curry powder" },
					new Ingredient { IngredientId = 6, Name = "tomato paste" },
					new Ingredient { IngredientId = 7, Name = "coconut milk" },
					new Ingredient { IngredientId = 8, Name = "water" },
					new Ingredient { IngredientId = 9, Name = "chicken" },
					new Ingredient { IngredientId = 10, Name = "lime juice" }
					);
				builder.Entity<Recipe>().HasData(
					new Recipe
					{
						RecipeId = 1,
						Title = "Coconut Curry Chicken",
						ServingNumber = 5,
						TimeNeededToPrepare = "35min",
						Description = "In a large pot or high-sided skillet over medium heat, heat oil and butter. When butter is" +
									" melted, add onion and shallots and cook until tender and translucent, 6 to 8 minutes.;Add garlic," +
									" ginger, and curry powder and cook until fragrant, 1 minute more. Add tomato paste and cook until" +
									" darkened slightly, 1 to 2 minutes more.;Add coconut milk and water and bring to a simmer. Add chicken " +
									"and cook, stirring occasionally, until chicken is cooked through, 6 to 8 minutes.;Stir in lime juice " +
									"and garnish with mint and cilantro. Serve hot with rice.",
						Note = "And don't forget to whip up some rice to soak up all that saucy goodness! ",
						CategoryId = 1,
						MealCanBeOrdered = true,
						Price = 10,
						NoteForShipping = "needs to be in freezer after deliver",
						UserId = 1,
						Complexity = "Simple"
					});
				builder.Entity<RecipeIngredients>().HasData(
					new RecipeIngredients { RecipeId = 1, Amount = "1 tbsp", IngredientId = 1 },
					new RecipeIngredients { RecipeId = 1, Amount = "1 tbsp", IngredientId = 2 },
					new RecipeIngredients { RecipeId = 1, Amount = "2 large", IngredientId = 3 },
					new RecipeIngredients { RecipeId = 1, Amount = "1 tbsp", IngredientId = 4 },
					new RecipeIngredients { RecipeId = 1, Amount = "1.5 tbsp", IngredientId = 5 },
					new RecipeIngredients { RecipeId = 1, Amount = "2 tbsp", IngredientId = 6 },
					new RecipeIngredients { RecipeId = 1, Amount = "1 can", IngredientId = 7 },
					new RecipeIngredients { RecipeId = 1, Amount = "500 ml", IngredientId = 8 },
					new RecipeIngredients { RecipeId = 1, Amount = "1.5 lb boneless", IngredientId = 9 },
					new RecipeIngredients { RecipeId = 1, Amount = "0.5ml", IngredientId = 10 }
				);
				builder.Entity<Order>().HasData(

					new Order
					{
						OrderId = 1,
						FullName = "Asija Ramovic",
						Address = "np",
						City = "novi pazar",
						State = "serbia",
						Zip = "36300",
						DateMealShouldBeShipped = new DateTime(2020, 3, 15),
						NoteToChef = "no pepper",
						ServingNumber = 5,
						RecipeId = 1
					},
					new Order
					{
						OrderId = 2,
						FullName = "Asija Ramovic",
						Address = "np",
						City = "novi pazar",
						State = "serbia",
						Zip = "36300",
						DateMealShouldBeShipped = new DateTime(2021, 3, 15),
						NoteToChef = "no pepper",
						ServingNumber = 10,
						RecipeId = 1
					}
				);
				builder.Entity<RecipeOrders>().HasData(
					new RecipeOrders { OrderId = 1, UserId = 2, ChefId = 1 },
					new RecipeOrders { OrderId = 2, UserId = 2, ApprovalStatus = "Approved", ChefId = 1 }
					);
			*/
		}
	}
}
