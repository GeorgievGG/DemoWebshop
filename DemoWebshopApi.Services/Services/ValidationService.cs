using DemoWebshopApi.Common;
using DemoWebshopApi.Data;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Data.Interfaces;
using DemoWebshopApi.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DemoWebshopApi.Services.Services
{

    public class ValidationService : IValidationService
    {
        private readonly WebshopContext _context;
        private readonly IIdentityUserManager _userManager;

        public ValidationService(WebshopContext context, IIdentityUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void EnsureMinLenghtIsValid(string forCheck, int length, string argumentName)
        {
            if (forCheck.Length <= length)
            {
                throw new InvalidLengthException(String.Format(Constants.InvalidLength, argumentName, length));
            }
        }

        public void EnsureEmailIsValid(string email)
        {
            if (!new EmailAddressAttribute().IsValid(email) || email.Length <= 4)
            {
                throw new InvalidEmailException(Constants.InvalidEmail);
            }
        }

        public async Task EnsureUsernameIsUniqueAsync(Guid id, string username)
        {
            var existingUser = await _userManager.FindByNameAsync(username);
            if (existingUser != null && existingUser.Id != id)
            {
                throw new EmailAlreadyInUseException(Constants.UsernameAreadyInUse);
            }
        }

        public async Task EnsureEmailIsUniqueAsync(Guid id, string email)
        {
            var users = await _userManager.GetAllAsync();
            if (users.Any(x => x.Id != id && x.Email == email))
            {
                throw new EmailAlreadyInUseException(Constants.EmailAreadyInUse);
            }
        }

        public void EnsureUserExist(User user)
        {
            if (user == null)
            {
                throw new EntityNotFoundException(String.Format(Constants.NotFound, "User"));
            }
        }

        public void EnsurePasswordsMatch(string newPassword, string repeatNewPassword)
        {
            if (newPassword != repeatNewPassword)
            {
                throw new PasswordsDontMatchException(Constants.PasswordsDontMatch);
            }
        }

        public async Task EnsureUserIsAdminAsync(User user)
        {
            if (await _userManager.IsUserInRole(user.Id, "Admin"))
            {
                throw new UserAlreadyAnAdminException(Constants.UserIsAdmin);
            }
        }

        public void EnsureUserDoesntHaveOrders(User user)
        {
            if (user.Orders.Count > 0)
            {
                throw new UserHasDependenciesException(Constants.UserHasDependenciesOrder);
            }
        }

        public void EnsureUserDoesntHaveBasket(User user)
        {
            if (user.Basket != null)
            {
                throw new UserHasDependenciesException(Constants.UserHasDependenciesBasket);
            }
        }
    }
}