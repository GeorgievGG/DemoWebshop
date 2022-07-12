using AutoMapper;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.DTOs.RequestModels;
using DemoWebshopApi.DTOs.ResponseModels;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpGet]
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
        public async Task<ActionResult<OrderResponseDto>> CreateOrder(OrderRequestDto order)
        {
            var toBeCreated = _mapper.Map<Order>(order);
            toBeCreated.OrderDate = DateTime.UtcNow;
            // TODO: Fix ClientId
            // TODO: Fix OrderId if needed
            var newOrder = await _orderService.CreateOrder(toBeCreated);

            return CreatedAtAction("GetOrder", new { id = newOrder.Id }, _mapper.Map<OrderResponseDto>(newOrder));
        }

        [HttpPut("{id}/SetDeliveryDate")]
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
