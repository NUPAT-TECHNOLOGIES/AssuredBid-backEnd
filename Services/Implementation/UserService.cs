using AssuredBid.Data;
using AssuredBid.DTOs;
using AssuredBid.Models;
using AssuredBid.Services.Iservice;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AssuredBid.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IMapper mapper;

        public UserService(ApplicationDbContext DbContext, IMapper mapper)
        {
            this.DbContext = DbContext;
            this.mapper = mapper;
        }

        public async Task AddUserAsync(UsersDTO userDto)
        {
            var user = mapper.Map<UsersPage>(userDto);
            await DbContext.AdminUsers.AddAsync(user);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await DbContext.AdminUsers.FindAsync(id);
            if (user != null)
            {
                DbContext.AdminUsers.Remove(user);
                await DbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UsersPage>> GetAllUsersAsync()
        {
            return await DbContext.AdminUsers.ToListAsync();
        }

        public async Task<UsersPage> GetUserByIdAsync(Guid id)
        {
            return await DbContext.AdminUsers.FindAsync(id);
        }

        public async Task<IEnumerable<UsersPage>> SearchUsersAsync(string query)
        {
            return await DbContext.AdminUsers
                .Where(u => u.Fullname.Contains(query) || u.EmailAddress.Contains(query))
                .ToListAsync();
        }

        public async Task UpdateUserAsync(UsersPage user)
        {
            DbContext.AdminUsers.Update(user);
            await DbContext.SaveChangesAsync();
        }
    }
}
