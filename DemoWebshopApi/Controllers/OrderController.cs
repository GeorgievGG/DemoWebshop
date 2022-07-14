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
    public class OrderController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ICollection<OrderResponseDto>>(orders));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }

            return _mapper.Map<OrderResponseDto>(order);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder(OrderRequestDto order)
        {
            if (UserId == null)
            {
                return BadRequest();
            }

            var toBeCreated = _mapper.Map<Order>(order);
            toBeCreated.OrderDate = DateTime.UtcNow;
            toBeCreated.ClientId = Guid.Parse(UserId);
            var newOrder = await _orderService.CreateOrder(toBeCreated);

            return CreatedAtAction("GetOrder", new { id = newOrder.Id }, _mapper.Map<OrderResponseDto>(newOrder));
        }

        [HttpPut("{id}/SetDeliveryDate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetDeliveryDate(Guid id, DateTime deliveryDate)
        {
            var isSuccessful = await _orderService.SetDeliveryDate(id, deliveryDate);
            if (!isSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}/ConfirmOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmOrder(Guid id)
        {
            var isSuccessful = await _orderService.ConfirmOrder(id);
            if (!isSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderService.DeleteOrder(order);

            return NoContent();
        }
    }
}
