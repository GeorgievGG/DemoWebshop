using DemoWebshopApi.Data;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoWebshopApi.Services.Services
{
    public class ShoppingBasketLineService : IShoppingBasketLineService
    {
        private readonly WebshopContext _context;
        private readonly IValidationService _validationService;
        private readonly IShoppingBasketService _shoppingBasketService;

        public ShoppingBasketLineService(
            WebshopContext context, 
            IValidationService validationService, 
            IShoppingBasketService shoppingBasketService)
        {
            _context = context;
            _validationService = validationService;
            _shoppingBasketService = shoppingBasketService;
        }

        public async Task<ShoppingBasketLine> GetShoppingBasketLine(Guid id)
        {
            var basketLine = await _context.ShoppingBasketLines.FindAsync(id);
            _validationService.EnsureNotNull(basketLine, nameof(basketLine));

            return basketLine;
        }

        public async Task<ShoppingBasket> ChangeBasketLineQuantity(Guid userId, ShoppingBasketLine shoppingBasketLine)
        {
            var shoppingBasket = await _shoppingBasketService.GetShoppingBasket(userId);
            if (shoppingBasket == null && shoppingBasketLine.Quantity > 0)
            {
                shoppingBasket = await _shoppingBasketService.CreateShoppingBasket(userId);
            }

            _validationService.EnsureNotNull(shoppingBasket, nameof(shoppingBasket));
            _validationService.EnsureNotNull(shoppingBasket.BasketLines, nameof(shoppingBasket.BasketLines));

            var basketLine = shoppingBasket.BasketLines.FirstOrDefault(x => x.ProductId == shoppingBasketLine.ProductId);
            if (basketLine != null)
            {
                basketLine.Quantity += shoppingBasketLine.Quantity;
                if (basketLine.Quantity <= 0)
                {
                    shoppingBasket.BasketLines.Remove(basketLine);
                    _context.ShoppingBasketLines.Remove(basketLine);
                }
            }
            else
            {
                _validationService.EnsureValueIsGreater(shoppingBasketLine.Quantity, 0, nameof(shoppingBasketLine.Quantity));
                shoppingBasket.BasketLines.Add(shoppingBasketLine);
            }

            await _context.SaveChangesAsync();

            return shoppingBasket;
        }


        public async Task DeleteShoppingBasketLine(Guid clientId, Guid lineId)
        {
            var shoppingBasket = await _context.ShoppingBaskets.FirstOrDefaultAsync(x => x.ClientId == clientId);
            _validationService.EnsureNotNull(shoppingBasket, nameof(shoppingBasket));

            var shoppingBasketLine = await _context.ShoppingBasketLines.FindAsync(lineId);
            _validationService.EnsureNotNull(shoppingBasketLine, nameof(shoppingBasketLine));

            _context.ShoppingBasketLines.Remove(shoppingBasketLine);

            await _context.SaveChangesAsync();
        }
    }
}
