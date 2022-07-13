using DemoWebshopApi.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DemoWebshopApi.Data.Interfaces
{
    public interface IIdentityUserManager
    {
        Task<IdentityResult> AddUserToRoleAsync(User user, string password);
        Task<bool> VerifyEmail(string email);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> DeleteUserAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User> GetUserAsync(ClaimsPrincipal principal);
        Task<List<string>> GetUserRolesAsync(User user);
        Task<bool> IsUserInRole(Guid userId, string roleName);
        Task<bool> ValidateUserCredentials(string userName, string password);
        Task<IdentityResult> UpdateUserDataAsync(User user);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        Task<User> FindByIdAsync(string id);
        Task<User> FindByNameAsync(string userName);
    }
}