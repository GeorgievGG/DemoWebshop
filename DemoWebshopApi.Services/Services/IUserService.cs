using DemoWebshopApi.Data.Entities;
using System.Security.Claims;

namespace DemoWebshopApi.Services.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(Guid id);
        Task<User> GetCurrentUser(ClaimsPrincipal principal);
        Task<User> CreateUser(User user, string password);
        Task<bool> UpdateUser(User user);
        Task<bool> SetUserInRole(Guid userId, string roleName);
        Task<bool> DeleteUser(Guid id);
        Task<bool> UpdateUserPasswrod(Guid id, UpdatePasswordDto updatePasswordDto);
    }
}