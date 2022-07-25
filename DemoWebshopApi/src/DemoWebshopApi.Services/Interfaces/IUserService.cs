using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Services;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(Guid id);
        Task<IList<User>> GetUsersInRole(string roleName);
        Task<User> CreateUser(User user, string password, string confirmPassword);
        Task UpdateUser(User user);
        Task MakeUserAdmin(Guid userId);
        Task UpdateUserPasswrod(Guid id, UpdatePasswordDto updatePasswordDto);
        Task DeleteUser(Guid id);
    }
}