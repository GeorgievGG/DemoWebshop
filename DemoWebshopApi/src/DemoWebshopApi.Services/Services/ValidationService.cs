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

        public void EnsureNotNull<T>(T input, string argumentName)
        {
            if (input == null)
            {
                throw new EntityNotFoundException(String.Format(Constants.NotFound, argumentName));
            }
        }

        public void EnsurePasswordsMatch(string newPassword, string repeatNewPassword)
        {
            if (newPassword != repeatNewPassword)
            {
                throw new PasswordsDontMatchException(Constants.PasswordsDontMatch);
            }
        }

        public async Task EnsureUserExists(Guid userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId.ToString());
            if (existingUser == null)
            {
                throw new UserNotExistException(Constants.ClientDoesNotExist);
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
                throw new UserHasDependenciesException(string.Format(Constants.ObjectHasDependencies, "user", "order"));
            }
        }

        public void EnsureUserDoesntHaveBasket(User user)
        {
            if (user.Basket != null)
            {
                throw new UserHasDependenciesException(string.Format(Constants.ObjectHasDependencies, "user", "basket"));
            }
        }

        public void EnsureProductUnique(Product product)
        {
            if (_context.Products.Any(x => x.Id != product.Id &&
                                        x.Name == product.Name &&
                                        x.Model == product.Model))
            {
                throw new ProductAlreadyExistsException(Constants.ProductAlreadyExists);
            }
        }

        public void EnsureValueIsNotEqual<T>(T value, T comparison, string argumentName) where T : struct
        {
            if (value.Equals(comparison))
            {
                throw new ValueInvalidException(string.Format(Constants.ValueIsNotValid, value, argumentName));
            }
        }

        public void EnsureValueIsGreater<T>(T value, T comparison, string argumentName) where T : IComparable<T>
        {
            if (value.CompareTo(comparison) <= 0)
            {
                throw new ValueInvalidException(string.Format(Constants.ValueIsNotValid, value, argumentName));
            }
        }

        public async Task EnsureProductExists(Guid id)
        {
            if (await _context.Products.FindAsync(id) == null)
            {
                throw new EntityNotFoundException(string.Format(Constants.NotFound, "Product"));
            }
        }

        public void EnsureProductDoesntHaveBaskets(Guid productId)
        {
            if (_context.ShoppingBasketLines.Any(x => x.ProductId == productId))
            {
                throw new UserHasDependenciesException(string.Format(Constants.ObjectHasDependencies, "product", "basket"));
            }
        }

        public void EnsureProductDoesntHaveOrders(Guid productId)
        {
            if (_context.OrderLines.Any(x => x.ProductId == productId))
            {
                throw new UserHasDependenciesException(string.Format(Constants.ObjectHasDependencies, "product", "order"));
            }
        }

        public void EnsureOrderConfirmed(Order order)
        {
            if (!order.Confirmed)
            {
                throw new OrderNotConfirmedException(Constants.OrderNotConfirmed);
            }
        }

        public void EnsureOrderLinesUnique(Order order)
        {
            var countByProduct = order.OrderLines.GroupBy(product => product.ProductId)
                        .Select(group => new
                        {
                            Key = group.Key,
                            Count = group.Count()
                        });
            if (countByProduct.Any(x => x.Count > 1))
            {
                throw new DuplicatedOrderLinesException(Constants.DuplicatedOrderLines);
            }
        }
    }
}