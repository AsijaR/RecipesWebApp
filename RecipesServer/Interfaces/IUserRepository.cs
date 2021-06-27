using RecipesServer.DTOs;
using RecipesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Interfaces
{
	public interface IUserRepository
	{
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        void deleteUserPreviousPhoto(int id);
        void CreateUserBookmark(int id);
       // Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);
       // Task<MemberDto> GetMemberAsync(string username);
    }
}
