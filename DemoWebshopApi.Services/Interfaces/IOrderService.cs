using DemoWebshopApi.Data.Entities;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(Order order);
        Task DeleteOrder(Order order);
        Task<Order> GetOrder(Guid id);
        Task<IEnumerable<Order>> GetOrders();
        Task<bool> UpdateOrder(Guid id, Order order);
    }
}