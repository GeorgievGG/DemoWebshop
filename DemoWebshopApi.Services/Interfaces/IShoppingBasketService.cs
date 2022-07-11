using DemoWebshopApi.Data.Entities;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IShoppingBasketService
    {
        Task<ShoppingBasket> GetShoppingBasket(Guid userId);
        Task<ShoppingBasket> CreateShoppingBasket(ShoppingBasket shoppingBasket);
        Task<bool> CreateShoppingBasketLine(Guid basketId, ShoppingBasketLine shoppingBasketLine);
        Task DeleteShoppingBasket(ShoppingBasket shoppingBasket);
    }
}