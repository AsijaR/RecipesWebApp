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
			services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
			services.AddTransient<IEmailService, EmailService>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IPhotoService, PhotoService>();
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
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
			services.AddDbContext<DataContext>(options =>
			{
				var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

				string connStr;

				// Depending on if in development or production, use either Heroku-provided
				// connection string, or development connection string from env var.
				if (env == "Development")
				{
					// Use connection string from file.
					connStr = config.GetConnectionString("DefaultConnection");
				}
				else
				{
					// Use connection string provided at runtime by Heroku.
					var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

					// Parse connection URL to connection string for Npgsql
					connUrl = connUrl.Replace("postgres://", string.Empty);
					var pgUserPass = connUrl.Split("@")[0];
					var pgHostPortDb = connUrl.Split("@")[1];
					var pgHostPort = pgHostPortDb.Split("/")[0];
					var pgDb = pgHostPortDb.Split("/")[1];
					var pgUser = pgUserPass.Split(":")[0];
					var pgPass = pgUserPass.Split(":")[1];
					var pgHost = pgHostPort.Split(":")[0];
					var pgPort = pgHostPort.Split(":")[1];

					connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";
				}

				// Whether the connection string came from the local development configuration file
				// or from the environment variable from Heroku, use it to set up your DbContext.
				options.UseNpgsql(connStr);
			});

			return services;
		}
	}
}
