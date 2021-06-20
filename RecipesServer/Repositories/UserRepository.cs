using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipesServer.Data;
using RecipesServer.Interfaces;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Repositories
{
	public class UserRepository : IUserRepository
	{
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

		public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<AppUser> GetUserByIdAsync(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<AppUser> GetUserByUsernameAsync(string username)
		{
			return await _context.Users
				.SingleOrDefaultAsync(x => x.UserName == username);
		}

		//public async Task<IEnumerable<AppUser>> GetUsersAsync()
		//{
		//	return await _context.Users.ToListAsync();
		//}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public void Update(AppUser user)
		{
			_context.Entry(user).State = EntityState.Modified;
		}
	}
}
