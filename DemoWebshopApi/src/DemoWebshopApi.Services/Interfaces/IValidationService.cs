using DemoWebshopApi.Data.Entities;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IValidationService
    {
        void EnsureMinLenghtIsValid(string forCheck, int length, string argumentName);
        void EnsureMaxLenghtIsValid(string forCheck, int length, string argumentName);
        void EnsureEmailIsValid(string email);
        Task EnsureUsernameIsUniqueAsync(Guid id, string username);
        Task EnsureEmailIsUniqueAsync(Guid id, string email);
        void EnsureNotNull<T>(T input, string argumentName);
        void EnsurePasswordsMatch(string newPassword, string repeatNewPassword);
        Task EnsureUserIsAdminAsync(User user);
        void EnsureUserDoesntHaveOrders(User user);
        void EnsureUserDoesntHaveBasket(User user);
        void EnsureProductUnique(Product product);
        void EnsureValueIsNotEqual<T>(T value, T comparison, string argumentName) where T : struct;
        void EnsureValueIsGreater<T>(T value, T comparison, string argumentName) where T : IComparable<T>;
        Task EnsureProductExists(Guid id);
        void EnsureProductDoesntHaveBaskets(Guid productId);
        void EnsureProductDoesntHaveOrders(Guid productId);
        void EnsureOrderConfirmed(Order order);
        void EnsureOrderLinesUnique(Order order);
        Task EnsureUserExists(Guid userId);
        void EnsureQuantityIsSufficient(ICollection<OrderLine> orderLines);
        void EnsureOrderHasLines(Order order);
        void EnsureOrderDatePrecedesDelivery(Order order, DateTime deliceryDate);
        void EnsureArrayNotEmpty<T>(T[] files, string listName);
    }
}