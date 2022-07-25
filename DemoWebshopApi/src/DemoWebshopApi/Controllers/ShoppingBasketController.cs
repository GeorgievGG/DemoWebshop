using AutoMapper;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.DTOs.RequestModels;
using DemoWebshopApi.DTOs.ResponseModels;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingBasketController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IShoppingBasketService _shoppingBasketService;
        private readonly IShoppingBasketLineService _shoppingBasketLineService;

        public ShoppingBasketController(
            IMapper mapper, 
            IShoppingBasketService shoppingBasketService,
            IShoppingBasketLineService shoppingBasketLineService)
        {
            _mapper = mapper;
            _shoppingBasketService = shoppingBasketService;
            _shoppingBasketLineService = shoppingBasketLineService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ShoppingBasketDetailedResponseDto>> GetShoppingBasket()
        {
            if (UserId == null)
            {
                return BadRequest();
            }
            var basket = await _shoppingBasketService.GetShoppingBasket(Guid.Parse(UserId));

            return _mapper.Map<ShoppingBasketDetailedResponseDto>(basket);
        }

        [HttpPost("IncreaseShoppingQuantity")]
        [Authorize]
        public async Task<ActionResult<ShoppingBasketResponseDto>> IncreaseShoppingBasketLineQuantity(Guid productId)
        {
            if (UserId == null)
            {
                return BadRequest();
            }

            var shoppingBasketLine = new ShoppingBasketLineRequestDto() { ProductId = productId, Quantity = 1 };
            var updatedShoppingBasket = await _shoppingBasketLineService.ChangeBasketLineQuantity(Guid.Parse(UserId), _mapper.Map<ShoppingBasketLine>(shoppingBasketLine));

            return CreatedAtAction("GetShoppingBasket", new { id = updatedShoppingBasket.Id }, _mapper.Map<ShoppingBasketResponseDto>(updatedShoppingBasket));
        }

        [HttpPost("DecreaseShoppingQuantity")]
        [Authorize]
        public async Task<ActionResult<ShoppingBasketResponseDto>> DecreaseShoppingBasketLineQuantity(Guid productId)
        {
            if (UserId == null)
            {
                return BadRequest();
            }

            var shoppingBasketLine = new ShoppingBasketLineRequestDto() { ProductId = productId, Quantity = -1 };
            var updatedShoppingBasket = await _shoppingBasketLineService.ChangeBasketLineQuantity(Guid.Parse(UserId), _mapper.Map<ShoppingBasketLine>(shoppingBasketLine));

            return CreatedAtAction("GetShoppingBasket", new { id = updatedShoppingBasket.Id }, _mapper.Map<ShoppingBasketResponseDto>(updatedShoppingBasket));
        }

        [HttpPost("SetShoppingQuantity")]
        [Authorize]
        public async Task<ActionResult<ShoppingBasketResponseDto>> SetShoppingBasketLineQuantity(ShoppingBasketLineRequestDto shoppingBasketLine)
        {
            if (UserId == null)
            {
                return BadRequest();
            }

            var updatedShoppingBasket = await _shoppingBasketLineService.ChangeBasketLineQuantity(Guid.Parse(UserId), _mapper.Map<ShoppingBasketLine>(shoppingBasketLine));

            return CreatedAtAction("GetShoppingBasket", new { id = updatedShoppingBasket.Id }, _mapper.Map<ShoppingBasketResponseDto>(updatedShoppingBasket));
        }

        [HttpDelete("DeleteShoppingBasketLine")]
        [Authorize]
        public async Task<IActionResult> DeleteShoppingBasketLine(Guid shoppingBasketLineId)
        {
            if (UserId == null)
            {
                return BadRequest();
            }

            await _shoppingBasketLineService.DeleteShoppingBasketLine(Guid.Parse(UserId), shoppingBasketLineId);

            return NoContent();
        }
    }
}
