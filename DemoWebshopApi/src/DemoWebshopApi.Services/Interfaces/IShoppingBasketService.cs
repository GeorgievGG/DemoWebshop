using DemoWebshopApi.Data.Entities;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IShoppingBasketService
    {
        Task<ShoppingBasket> GetShoppingBasket(Guid userId);
        Task<ShoppingBasket> CreateShoppingBasket(Guid userId);
        Task DeleteShoppingBasket(Guid userId);
    }
}