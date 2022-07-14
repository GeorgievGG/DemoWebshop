using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingBasketController : BaseController
    {
        private readonly IShoppingBasketService _shoppingBasketService;
        private readonly IShoppingBasketLineService _shoppingBasketLineService;

        public ShoppingBasketController(IShoppingBasketService shoppingBasketService,
            IShoppingBasketLineService shoppingBasketLineService)
        {
            _shoppingBasketService = shoppingBasketService;
            _shoppingBasketLineService = shoppingBasketLineService;
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<ActionResult<ShoppingBasket>> GetShoppingBasket(Guid userId)
        {
            return await _shoppingBasketService.GetShoppingBasket(userId);
        }

        [HttpPost("{userId}/AddShoppingBasketLine")]
        [Authorize]
        public async Task<ActionResult<ShoppingBasket>> CreateShoppingBasketLine(Guid userId, ShoppingBasketLine shoppingBasketLine)
        {
            var shoppingBasket = await _shoppingBasketService.GetShoppingBasket(userId);
            if (shoppingBasket == null)
            {
                var newBasket = new ShoppingBasket() { Id = Guid.NewGuid(), ClientId = userId };
                shoppingBasket = await _shoppingBasketService.CreateShoppingBasket(newBasket);
            }

            var isLineAddedSuccessfully = await _shoppingBasketLineService.CreateShoppingBasketLine(shoppingBasket.Id, shoppingBasketLine);
            if (!isLineAddedSuccessfully)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetShoppingBasket", new { id = shoppingBasket.Id }, shoppingBasket);
        }

        [HttpDelete("{userId}/DeleteShoppingBasketLine")]
        [Authorize]
        public async Task<IActionResult> DeleteShoppingBasketLine(Guid userId, ShoppingBasketLine shoppingBasketLine)
        {
            var shoppingBasket = await _shoppingBasketService.GetShoppingBasket(userId);
            if (shoppingBasket == null)
            {
                return NotFound();
            }

            var lineToDelete = await _shoppingBasketLineService.GetShoppingBasketLine(shoppingBasketLine.Id);
            if (lineToDelete == null)
            {
                return NotFound();
            }

            var isLineAddedSuccessfully = await _shoppingBasketLineService.DeleteShoppingBasketLine(shoppingBasket.Id, shoppingBasketLine.Id);
            if (!isLineAddedSuccessfully)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
