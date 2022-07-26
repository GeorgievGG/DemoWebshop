using DemoWebshopApi.Data;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoWebshopApi.Services.Services
{
    public class ShoppingBasketService : IShoppingBasketService
    {
        private readonly WebshopContext _context;
        private readonly IValidationService _validationService;

        public ShoppingBasketService(WebshopContext context, IValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        public async Task<ShoppingBasket> GetShoppingBasket(Guid userId)
        {
            var shoppingBasket = await _context.ShoppingBaskets
                .Include(x => x.BasketLines)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.ClientId == userId);
            if (shoppingBasket == null)
            {
                shoppingBasket = await CreateShoppingBasket(userId);
            }

            return shoppingBasket;
        }

        public async Task<ShoppingBasket> CreateShoppingBasket(Guid userId)
        {
            var newBasket = new ShoppingBasket() { ClientId = userId };
            _context.ShoppingBaskets.Add(newBasket);
            await _context.SaveChangesAsync();

            return newBasket;
        }
    }
}
