using DemoWebshopApi.Data.Entities;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(Guid id);
        Task<Order> CreateOrder(Order order);
        Task ConfirmOrder(Guid id);
        Task SetDeliveryDate(Guid id, DateTime date);
        Task DeleteOrder(Guid id);
    }
}