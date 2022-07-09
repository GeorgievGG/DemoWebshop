using DemoWebshopApi.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace DemoWebshopApi.Data.Interfaces
{
    public interface IIDentityUserManager
    {
        Task<List<User>> GetAllAsync();

        Task<List<string>> GetUserRolesAsync(User user);

        Task<IdentityResult> CreateUserAsync(User user, string password);

        Task<IdentityResult> DeleteUserAsync(User user);

        Task<IdentityResult> UpdateUserDataAsync(User user);

        Task<bool> VerifyEmail(string email);

        Task<bool> IsUserInRole(Guid userId, string roleName);

        Task<bool> ValidateUserCredentials(string userName, string password);

        Task<IdentityResult> AddUserToRoleAsync(User user, string password);
    }
}