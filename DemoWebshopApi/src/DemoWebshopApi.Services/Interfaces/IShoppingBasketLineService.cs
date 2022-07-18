using DemoWebshopApi.Data.Entities;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IShoppingBasketLineService
    {
        Task<ShoppingBasketLine> GetShoppingBasketLine(Guid id);
        Task<ShoppingBasket> ChangeBasketLineQuantity(Guid clientId, ShoppingBasketLine shoppingBasketLine);
        Task DeleteShoppingBasketLine(Guid clientId, Guid lineId);
    }
}