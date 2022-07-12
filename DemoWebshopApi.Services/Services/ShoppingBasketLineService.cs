using DemoWebshopApi.Data;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoWebshopApi.Services.Services
{
    public class ShoppingBasketLineService : IShoppingBasketLineService
    {
        private readonly WebshopContext _context;

        public ShoppingBasketLineService(WebshopContext context)
        {
            _context = context;
        }

        public async Task<ShoppingBasketLine> GetShoppingBasketLine(Guid id)
        {
            return await _context.ShoppingBasketLines.FirstOrDefaultAsync(x => x.Id == id);
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


        public async Task<bool> DeleteShoppingBasketLine(Guid basketId, Guid lineId)
        {
            var basket = await _context.ShoppingBaskets.FindAsync(basketId);
            if (basket == null)
            {
                return false;
            }

            var basketLine = await _context.ShoppingBasketLines.FindAsync(lineId);
            if (basketLine == null)
            {
                return false;
            }

            _context.ShoppingBasketLines.Remove(basketLine);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
