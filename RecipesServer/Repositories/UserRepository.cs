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
			return await _context.Users.Include(p => p.UserPhoto).FirstOrDefaultAsync(x => x.Id == id);
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
		public void deleteUserPreviousPhoto(int id)
		{
			var exist = _context.UserPhotos.Where(x => x.AppUserId == id).Any();
			if (exist)
			{
				var delPhoto = _context.UserPhotos.FirstOrDefault(x => x.AppUserId == id);
				_context.UserPhotos.Remove(delPhoto);
			}
		}
		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public void Update(AppUser user)
		{
			_context.Entry(user).State = EntityState.Modified;
		}
		public async void CreateUserBookmark(int id)
		{
			_context.Bookmarks.Add(new Bookmark() { UserId = id });
			await _context.SaveChangesAsync();
		}
	}
}
