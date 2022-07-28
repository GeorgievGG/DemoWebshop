using DemoWebshopApi.Data.Entities;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(Guid id);
        Task<Product> CreateProduct(Product product);
        Task UpdateProduct(Guid id, Product product);
        Task ReduceProductQuantities(ICollection<OrderLine> orderLines);
        Task DeleteProduct(Guid id);
    }
}