using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface IUnitOfWork
	{
		//IOrderRepository OrderRepository { get; }
		ICommentRepository CommentRepository { get; }
		IBookmarkRepository BookmarkRepository { get; }
		IRecipeRepository RecipeRepository { get; }
		IUserRepository UserRepository { get; }
		ICategoryRepository CategoryRepository { get; }
		Task<bool> Complete();
		bool HasChanges();
	}
}
