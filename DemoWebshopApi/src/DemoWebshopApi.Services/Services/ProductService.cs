using DemoWebshopApi.Data;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoWebshopApi.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly WebshopContext _context;
        private readonly IValidationService _validationService;

        public ProductService(WebshopContext context, IValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            _validationService.EnsureNotNull(product, nameof(product));

            return product;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            _validationService.EnsureMaxLenghtIsValid(product.Name, 120, nameof(product.Name));
            _validationService.EnsureMaxLenghtIsValid(product.Model, 120, nameof(product.Model));

            _validationService.EnsureProductUnique(product);
            _validationService.EnsureValueIsNotEqual(product.AvailableQuantity, 0, nameof(product.AvailableQuantity));
            _validationService.EnsureValueIsNotEqual(product.Price, 0, nameof(product.Price));
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task UpdateProduct(Guid id, Product product)
        {
            _validationService.EnsureMaxLenghtIsValid(product.Name, 120, nameof(product.Name));
            _validationService.EnsureMaxLenghtIsValid(product.Model, 120, nameof(product.Model));

            product.Id = id;
            await _validationService.EnsureProductExists(id);
            _validationService.EnsureProductUnique(product);
            _validationService.EnsureValueIsGreater(product.AvailableQuantity, -1, nameof(product.AvailableQuantity));
            _validationService.EnsureValueIsNotEqual(product.Price, 0, nameof(product.Price));
            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task ReduceProductQuantities(ICollection<OrderLine> orderLines)
        {
            var products = await GetProducts();
            foreach (var orderLine in orderLines)
            {
                var product = products.FirstOrDefault(x => x.Id == orderLine.ProductId);
                _validationService.EnsureNotNull(product, nameof(product));

                product.AvailableQuantity -= orderLine.Quantity;
                _context.Entry(product).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Guid id)
        {
            await _validationService.EnsureProductExists(id);
            _validationService.EnsureProductDoesntHaveBaskets(id);
            _validationService.EnsureProductDoesntHaveOrders(id);

            var product = await _context.Products.FindAsync(id);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
