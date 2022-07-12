using DemoWebshopApi.Data;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoWebshopApi.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly WebshopContext _context;

        public OrderService(WebshopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrder(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<bool> SetDeliveryDate(Guid id, DateTime date)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            order.DeliveryDate = date;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ConfirmOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            order.Confirmed = true;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        private bool OrderExists(Guid id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
