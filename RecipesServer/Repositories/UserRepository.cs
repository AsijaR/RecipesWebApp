using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipesServer.Data;
using RecipesServer.DTOs;
using RecipesServer.Helpers;
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

		public async Task<IEnumerable<UserInfoDTO>> GetAllUsersAsync()
		{
			var users= await _context.Users.OrderBy(u=>u.UserName).ToListAsync();
			return _mapper.Map<IEnumerable<UserInfoDTO>>(users);
		}

		public async Task<AppUser> GetUserByIdAsync(int id)
		{
			return await _context.Users.Include(p=>p.UserPhoto).FirstOrDefaultAsync(x => x.Id == id);
		}
		public bool deleteUser(int userId) 
		{
			var user =  _context.Users.FirstOrDefault(u=>u.Id==userId);
			if (user != null)
			{
				_context.Users.Remove(user);
				return true;
			}
			else return false;
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
		public void CreateUserBookmark(int id)
		{
			_context.Bookmarks.Add(new Bookmark() { UserId = id });
			_context.SaveChanges();
		}
	}
}
