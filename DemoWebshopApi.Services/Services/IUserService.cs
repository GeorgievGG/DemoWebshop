using DemoWebshopApi.Data.Entities;
using System.Security.Claims;

namespace DemoWebshopApi.Services.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetCurrentUser(ClaimsPrincipal principal);
        Task<User> CreateUserAsync(User user, string password);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> UpdateUserPasswrodAsync(Guid id, UpdatePasswordDto updatePasswordDto);
    }
}