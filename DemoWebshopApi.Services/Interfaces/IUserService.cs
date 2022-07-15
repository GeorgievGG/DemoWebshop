using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Services;
using System.Security.Claims;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(Guid id);
        Task<User> CreateUser(User user, string password);
        Task UpdateUser(User user);
        Task SetUserInRole(Guid userId, string roleName);
        Task DeleteUser(Guid id);
        Task UpdateUserPasswrod(Guid id, UpdatePasswordDto updatePasswordDto);
    }
}