using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipesServer.Data;
using RecipesServer.Helpers;
using RecipesServer.Interfaces;
using RecipesServer.Repositories;
using RecipesServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RecipesServer.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IRecipePhotoService, RecipePhotoService>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IRecipeRepository, RecipeRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IBookmarkRepository, BookmarkRepository>();
			services.AddScoped<ICommentRepository, CommentRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			//services.AddScoped<ILikesRepository, LikesRepository>();
			//services.AddScoped<MessageRepository, MessageRepository>();
			//services.AddScoped<LogUserActivity>();
			//services.AddScoped<IUserRepository, UserRepository>();
			services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
			services.AddDbContext<DataContext>(options =>
			{
				options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
			});

			return services;
		}
	}
}
