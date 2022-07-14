﻿using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Data.Interfaces;
using System.Security.Claims;

namespace DemoWebshopApi.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityUserManager _userManager;

        public UserService(IIdentityUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userManager.GetAllAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            var checkUser = await _userManager.FindByIdAsync(id.ToString());

            return checkUser;
        }

        public async Task<User> CreateUser(User user, string password)
        {
            await _userManager.CreateUserAsync(user, password);
            await _userManager.AddUserToRoleAsync(user, "User");

            return await _userManager.FindByNameAsync(user.UserName);
        }

        public async Task<bool> UpdateUser(User user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (existingUser == null)
            {
                return false;
            }

            existingUser.UserName = user.UserName;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            await _userManager.UpdateUserDataAsync(existingUser);

            return true;
        }

        public async Task<bool> UpdateUserPasswrod(Guid id, UpdatePasswordDto updatePasswordDto)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if (existingUser == null || updatePasswordDto.NewPassword != updatePasswordDto.RepeatNewPassword)
            {
                return false;
            }

            await _userManager.ChangePasswordAsync(existingUser, updatePasswordDto.CurrentPassword, updatePasswordDto.NewPassword);

            return true;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var checkUser = await _userManager.FindByIdAsync(id.ToString());

            await _userManager.DeleteUserAsync(checkUser);
            return true;
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task<bool> SetUserInRole(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            await _userManager.AddUserToRoleAsync(user, roleName);

            return true;
        }
    }
}
