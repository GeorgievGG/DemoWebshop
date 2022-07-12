﻿using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }

            return order;
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

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            await _orderService.CreateOrder(order);

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
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
