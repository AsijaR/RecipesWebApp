using AutoMapper;
using RecipesServer.Interfaces;
using RecipesServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
		public UnitOfWork(DataContext context, IMapper mapper, IEmailService emailService)
		{
			_context = context;
			_mapper = mapper;
			_emailService = emailService;
		}

		public IRecipeRepository RecipeRepository => new RecipeRepository(_context,_mapper);

		public IUserRepository UserRepository => new UserRepository(_context, _mapper);

		public ICategoryRepository CategoryRepository => new CategoryRepository(_context, _mapper);
		public IBookmarkRepository BookmarkRepository => new BookmarkRepository(_context, _mapper);
		public ICommentRepository CommentRepository => new CommentRepository(_context, _mapper);
		public IOrderRepository OrderRepository => new OrderRepository(_context, _mapper,_emailService);

		public async Task<bool> Complete()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public bool HasChanges()
		{
			_context.ChangeTracker.DetectChanges();
			var changes = _context.ChangeTracker.HasChanges();

			return changes;
		}
	}
}
