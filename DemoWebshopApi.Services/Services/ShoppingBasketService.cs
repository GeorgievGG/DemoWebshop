using DemoWebshopApi.Data;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoWebshopApi.Services.Services
{
    public class ShoppingBasketService : IShoppingBasketService
    {
        private readonly WebshopContext _context;

        public ShoppingBasketService(WebshopContext context)
        {
            _context = context;
        }

        public async Task<ShoppingBasket> GetShoppingBasket(Guid userId)
        {
            return await _context.ShoppingBaskets.FirstOrDefaultAsync(x => x.ClientId == userId);
        }

        public async Task<ShoppingBasket> CreateShoppingBasket(ShoppingBasket shoppingBasket)
        {
            _context.ShoppingBaskets.Add(shoppingBasket);
            await _context.SaveChangesAsync();

            return shoppingBasket;
        }

        public async Task<bool> CreateShoppingBasketLine(Guid basketId, ShoppingBasketLine shoppingBasketLine)
        {
            var basket = await _context.ShoppingBaskets.FindAsync(basketId);
            if (basket == null)
            {
                return false;
            }

            basket.BasketLines.Add(shoppingBasketLine);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteShoppingBasket(ShoppingBasket shoppingBasket)
        {
            _context.ShoppingBaskets.Remove(shoppingBasket);
            await _context.SaveChangesAsync();
        }
    }
}
