using DemoWebshopApi.Data;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoWebshopApi.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly WebshopContext _context;
        private readonly IValidationService _validationService;
        private readonly IProductService _productService;

        public OrderService(WebshopContext context, IValidationService validationService, IProductService productService)
        {
            _context = context;
            _validationService = validationService;
            _productService = productService;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders.Include(x => x.OrderLines).Include(x => x.Client).ToListAsync();
        }

        public async Task<Order> GetOrder(Guid id)
        {
            var order = await _context.Orders.Include(x => x.OrderLines).Include(x => x.Client).FirstOrDefaultAsync(x => x.Id == id);
            _validationService.EnsureNotNull(order, nameof(order));

            return order;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _validationService.EnsureUserExists(order.ClientId);
            _validationService.EnsureOrderLinesUnique(order);
            _validationService.EnsureQuantityIsSufficient(order.OrderLines);
            await _productService.ReduceProductQuantities(order.OrderLines);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task ConfirmOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            _validationService.EnsureNotNull(order, nameof(order));

            order.Confirmed = true;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task SetDeliveryDate(Guid id, DateTime date)
        {
            var order = await _context.Orders.FindAsync(id);
            _validationService.EnsureNotNull(order, nameof(order));
            _validationService.EnsureOrderConfirmed(order);

            order.DeliveryDate = date;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            _validationService.EnsureNotNull(order, nameof(order));

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
