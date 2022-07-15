using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Data.Interfaces;
using DemoWebshopApi.Services.Interfaces;

namespace DemoWebshopApi.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityUserManager _userManager;
        private readonly IValidationService _validationService;

        public UserService(IIdentityUserManager userManager, IValidationService validationService)
        {
            _userManager = userManager;
            _validationService = validationService;
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
            _validationService.EnsureMinLenghtIsValid(password, 7, nameof(password));
            _validationService.EnsureMinLenghtIsValid(user.FirstName, 2, nameof(user.FirstName));
            _validationService.EnsureMinLenghtIsValid(user.LastName, 2, nameof(user.LastName));

            _validationService.EnsureEmailIsValid(user.Email);
            await _validationService.EnsureEmailIsUniqueAsync(user.Email);

            await _userManager.CreateUserAsync(user, password);
            await _userManager.AddUserToRoleAsync(user, "User");

            return await _userManager.FindByNameAsync(user.UserName);
        }

        public async Task UpdateUser(User user)
        {
            _validationService.EnsureMinLenghtIsValid(user.FirstName, 2, nameof(user.FirstName));
            _validationService.EnsureMinLenghtIsValid(user.LastName, 2, nameof(user.FirstName));

            var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
            _validationService.EnsureUserExist(existingUser);

            _validationService.EnsureEmailIsValid(user.Email);
            await _validationService.EnsureEmailIsUniqueAsync(user.Email);

            existingUser.UserName = user.UserName;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            await _userManager.UpdateUserDataAsync(existingUser);
        }

        public async Task UpdateUserPasswrod(Guid id, UpdatePasswordDto updatePasswordDto)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            _validationService.EnsureUserExist(existingUser);
            _validationService.EnsurePasswordsMatch(updatePasswordDto.NewPassword, updatePasswordDto.RepeatNewPassword);
            _validationService.EnsureMinLenghtIsValid(updatePasswordDto.NewPassword, 7, nameof(updatePasswordDto.NewPassword));

            await _userManager.ChangePasswordAsync(existingUser, updatePasswordDto.CurrentPassword, updatePasswordDto.NewPassword);
        }

        public async Task DeleteUser(Guid id)
        {
            var checkUser = await _userManager.FindByIdAsync(id.ToString());
            _validationService.EnsureUserExist(checkUser);
            _validationService.EnsureUserDoesntHaveOrders(checkUser);
            _validationService.EnsureUserDoesntHaveBasket(checkUser);

            await _userManager.DeleteUserAsync(checkUser);
        }

        public async Task SetUserInRole(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            _validationService.EnsureUserExist(user);
            await _validationService.EnsureUserIsAdminAsync(user);

            await _userManager.AddUserToRoleAsync(user, roleName);
        }
    }
}
