﻿using AutoMapper;
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
            toBeCreated.OrderDate = DateTime.UtcNow.Date;
            toBeCreated.ClientId = Guid.Parse(UserId);
            var newOrder = await _orderService.CreateOrder(toBeCreated);

            return CreatedAtAction("GetOrder", new { id = newOrder.Id }, _mapper.Map<OrderResponseDto>(newOrder));
        }

        [HttpPut("{id}/ConfirmOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmOrder(Guid id)
        {
            await _orderService.ConfirmOrder(id);

            return NoContent();
        }

        [HttpPut("{id}/SetDeliveryDate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetDeliveryDate(Guid id, SetDeliveryDateInput input)
        {
            await _orderService.SetDeliveryDate(id, input.DeliveryDate.Date);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            await _orderService.DeleteOrder(id);

            return NoContent();
        }
    }
}
