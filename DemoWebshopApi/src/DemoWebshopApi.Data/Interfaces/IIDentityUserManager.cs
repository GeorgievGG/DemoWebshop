using DemoWebshopApi.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DemoWebshopApi.Data.Interfaces
{
    public interface IIdentityUserManager
    {
        Task<List<User>> GetAllAsync();
        Task<User> FindByIdAsync(Guid id);
        Task<User> FindByNameAsync(string userName);
        Task<User> GetUserAsync(ClaimsPrincipal principal);
        Task<IList<User>> GetUsersInRoleAsync(string roleName);
        Task<List<string>> GetUserRolesAsync(User user);
        Task<bool> ValidateUserCredentials(string userName, string password);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> AddUserToRoleAsync(User user, string role);
        Task<IdentityResult> RemoveUserFromRoleAsync(User user, string role);
        Task<bool> IsUserInRole(Guid userId, string roleName);
        Task<bool> CheckIfEmailExists(string email);
        Task<IdentityResult> UpdateUserDataAsync(User user);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        Task<IdentityResult> DeleteUserAsync(User user);
    }
}