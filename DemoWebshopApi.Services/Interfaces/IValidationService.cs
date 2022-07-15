using DemoWebshopApi.Data.Entities;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IValidationService
    {
        void EnsureMinLenghtIsValid(string forCheck, int length, string argumentName);
        void EnsureEmailIsValid(string email);
        Task EnsureEmailIsUniqueAsync(string email);
        void EnsureUserExist(User user);
        void EnsurePasswordsMatch(string newPassword, string repeatNewPassword);
        Task EnsureUserIsAdminAsync(User user);
        void EnsureUserDoesntHaveOrders(User user);
        void EnsureUserDoesntHaveBasket(User user);
    }
}