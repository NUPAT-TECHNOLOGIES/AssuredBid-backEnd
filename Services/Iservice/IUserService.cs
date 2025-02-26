using AssuredBid.DTOs;
using AssuredBid.Models;

namespace AssuredBid.Services.Iservice
{
    public interface IUserService
    {
        Task<IEnumerable<UsersPage>> GetAllUsersAsync();
        Task<UsersPage> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UsersPage>> SearchUsersAsync(string query);
        Task AddUserAsync(UsersDTO userDto);
        Task UpdateUserAsync(UsersPage user);
        Task DeleteUserAsync(Guid id);
    }
}
