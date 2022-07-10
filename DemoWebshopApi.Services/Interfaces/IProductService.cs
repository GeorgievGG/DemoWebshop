using DemoWebshopApi.Data.Entities;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product product);
        Task DeleteProduct(Product product);
        Task<Product> GetProduct(Guid id);
        Task<IEnumerable<Product>> GetProducts();
        Task<bool> UpdateProduct(Guid id, Product product);
    }
}