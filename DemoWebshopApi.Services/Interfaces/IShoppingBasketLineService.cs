using DemoWebshopApi.Data.Entities;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IShoppingBasketLineService
    {
        Task<ShoppingBasketLine> GetShoppingBasketLine(Guid id);
        Task<bool> CreateShoppingBasketLine(Guid basketId, ShoppingBasketLine shoppingBasketLine);
        Task<bool> DeleteShoppingBasketLine(Guid basketId, Guid lineId);
    }
}