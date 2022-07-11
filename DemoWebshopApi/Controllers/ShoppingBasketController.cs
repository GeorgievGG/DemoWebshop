using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingBasketController : ControllerBase
    {
        private readonly IShoppingBasketService _shoppingBasketService;

        public ShoppingBasketController(IShoppingBasketService shoppingBasketService)
        {
            _shoppingBasketService = shoppingBasketService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ShoppingBasket>> GetShoppingBasket(Guid userId)
        {
            return await _shoppingBasketService.GetShoppingBasket(userId);
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingBasket>> CreateShoppingBasketLine(Guid userId, ShoppingBasketLine shoppingBasketLine)
        {
            var shoppingBasket = await _shoppingBasketService.GetShoppingBasket(userId);
            if (shoppingBasket == null)
            {
                var newBasket = new ShoppingBasket() { Id = Guid.NewGuid(), ClientId = userId };
                shoppingBasket = await _shoppingBasketService.CreateShoppingBasket(newBasket);
            }

            var isLineAddedSuccessfully = await _shoppingBasketService.CreateShoppingBasketLine(shoppingBasket.Id, shoppingBasketLine);
            if (!isLineAddedSuccessfully)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetShoppingBasket", new { id = shoppingBasket.Id }, shoppingBasket);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingBasket(Guid id)
        {
            var shoppingBasket = await _shoppingBasketService.GetShoppingBasket(id);
            if (shoppingBasket == null)
            {
                return NotFound();
            }

            await _shoppingBasketService.DeleteShoppingBasket(shoppingBasket);

            return NoContent();
        }
    }
}
